import React, {useCallback, useEffect, useState, useMemo} from 'react'
import {
  ScrollView,
  StyleSheet,
  View,
  Pressable,
  Dimensions,
  Text
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  AlertComponent,
  ReviewComponentPlusIcon,
  TouchableView,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {ParamList} from '../navigator/MainNavigator'
import {post} from '../server'
import type {Review} from '../type'
import Color from 'color'
import FastImage from 'react-native-fast-image'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  absoluteView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    backgroundColor: Color('#000000').alpha(0.45).string()
  },
  selectBar: {
    width: '100%',
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20
  },
  selectItem: {
    width: Dimensions.get('window').width - 40,
    height: 70,
    marginLeft: 20,
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    alignItems: 'center'
  },
  selectItemText: {
    marginLeft: 10,
    fontSize: 14
  },
  floatingButton: {
    position: 'absolute',
    bottom: 20,
    right: 20,
    width: 60,
    height: 60,
    borderRadius: 30,
    backgroundColor: '#E6135A',
    alignItems: 'center',
    justifyContent: 'center'
  }
})

export default function ReviewScreen() {
  const [reviewList, setReviewList] = useState<Review[]>([])
  const [errorMessage, setErrorMessage] = useState<string>('')
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false)
  const [reportTarget, setReportTarget] = useState<number>(-1)
  const [showSelectBar, setShowSelectBar] = useState<boolean>(false)
  const [showSelectDeleteBar, setShowSelectDeleteBar] = useState<boolean>(false)
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'ReviewScreen'>>()
  const NickName = route.params.NickName
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const ReviewComponentList = useMemo(() => {
    return reviewList.map((item, index) => (
      <ReviewComponentPlusIcon
        reviewInfo={item}
        key={index.toString()}
        iconPress={() => {
          setReportTarget(item.Index_num)
          if (item.WriterID === user.ID) {
            setShowSelectDeleteBar(true)
          } else {
            setShowSelectBar(true)
          }
        }}
      />
    ))
  }, [reviewList, user])

  const reportAlertPress = useCallback(() => {
    let data = new FormData()
    data.append('Target', 'Review')
    data.append('TargetID', reportTarget)
    post('InsertReport.php', data)
    setShowReportAlert(false)
    setErrorMessage('신고가 접수 되었습니다.')
    setShowErrorAlert(true)
  }, [reportTarget])

  const deleteReview = useCallback(() => {
    let data = new FormData()
    data.append('Index_num', reportTarget)
    post('DeleteReview.php', data)
      .then(() => {
        setErrorMessage('삭제가 완료되었습니다.')
        setShowErrorAlert(true)
      })
      .catch((e) => {
        setErrorMessage('리뷰 삭제 진행에 오류가 발생했습니다.')
        setShowErrorAlert(true)
      })
  }, [reportTarget])

  useEffect(() => {
    let data = new FormData()
    data.append('NickName', NickName)
    post('GetReviewList.php', data)
      .then((ReviewListInfo) => ReviewListInfo.json())
      .then((ReviewListInfoObject) => {
        const {GetReviewList} = ReviewListInfoObject
        setReviewList(GetReviewList)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setErrorMessage('후기 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setLoading(false)
      })
  }, [NickName])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
        <SafeAreaView style={[styles.view]}>
          <NavigationHeader
            viewStyle={[{backgroundColor: 'white'}]}
            Left={() => (
              <Icon
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
            title="판매자 후기 보기"
          />
          <ScrollView contentContainerStyle={{flex: 1, paddingHorizontal: 20}}>
            {ReviewComponentList}
          </ScrollView>
        </SafeAreaView>
      )}
      {user.ID !== 'guest' && (
        <Pressable
          style={[styles.floatingButton]}
          onPress={() => navigation.navigate('WriteReview', {NickName})}>
          <FastImage
            style={[{width: 30, height: 30}]}
            source={require('../assets/images/modify_white.png')}
          />
        </Pressable>
      )}
      {showErrorAlert && (
        <AlertComponent
          cancelUse={false}
          message={errorMessage}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
      {showReportAlert && (
        <AlertComponent
          cancelUse={true}
          message="신고하시겠습니까?"
          cancelPress={() => setShowReportAlert(false)}
          okPress={reportAlertPress}
        />
      )}
      {showSelectBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowSelectBar(false)}
          />
          <View style={[styles.selectBar]}>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setShowSelectBar(false)
                setShowReportAlert(true)
              }}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/report_background.png')}
              />
              <Text style={[styles.selectItemText]}>신고하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {showSelectDeleteBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowSelectDeleteBar(false)}
          />
          <View style={[styles.selectBar]}>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setShowSelectDeleteBar(false)
                deleteReview()
              }}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/delete_background.png')}
              />
              <Text style={[styles.selectItemText]}>삭제하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
    </>
  )
}
