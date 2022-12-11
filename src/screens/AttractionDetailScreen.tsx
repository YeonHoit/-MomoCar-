import React, {useCallback, useMemo, useRef, useEffect, useState} from 'react'
import {
  StyleSheet,
  View,
  ScrollView,
  Dimensions,
  ImageBackground,
  Text,
  Linking,
  Pressable
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon as IconNative,
  AttractionSupport,
  Loading,
  ReviewComponentPlusIcon,
  TouchableView,
  AlertComponent
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {ParamList} from '../navigator/MainNavigator'
import {serverUrl} from '../server'
import Swiper from 'react-native-swiper'
import NaverMapView, {Coord, Marker} from 'react-native-nmap'
import FastImage from 'react-native-fast-image'
import Icon from 'react-native-vector-icons/MaterialCommunityIcons'
import Clipboard from '@react-native-clipboard/clipboard'
import Toast from 'react-native-easy-toast'
import {post} from '../server'
import type {AttractionReview} from '../type'
import Color from 'color'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  swiperdot: {
    backgroundColor: '#EDEDED',
    height: 2,
    borderRadius: 4,
    marginLeft: 3,
    marginRight: 3,
    marginTop: 3,
    marginBottom: -20
  },
  activedot: {
    backgroundColor: '#E6135A',
    height: 2,
    borderRadius: 4,
    marginLeft: 3,
    marginRight: 3,
    marginTop: 3,
    marginBottom: -20
  },
  idText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
    marginLeft: 20,
    marginBottom: 20,
    fontStyle: 'italic'
  },
  normalView: {
    width: '100%',
    paddingHorizontal: 20,
    paddingVertical: 15,
    marginTop: 10,
    backgroundColor: 'white'
  },
  mapView: {
    width: '100%',
    height: 200,
    marginTop: 10
  },
  no_review: {
    width: '100%',
    height: 300,
    justifyContent: 'center',
    alignItems: 'center'
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
  }
})

// 명소 자세히 보기 화면
export default function AttractionDetailScreen() {
  const [reviewList, setReviewList] = useState<AttractionReview[]>([]) // 명소 리뷰 리스트 배열
  const [reviewLoading, setReviewLoading] = useState<boolean>(true) // 리뷰 로딩 여부
  const [showSelectBar, setShowSelectBar] = useState<boolean>(false) // 신고 바 표시 여부
  const [showSelectDeleteBar, setShowSelectDeleteBar] = useState<boolean>(false) // 삭제 바 표시 여부
  const [reportTarget, setReportTarget] = useState<number>(-1) // 신고 및 삭제 타겟
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false) // 신고 여부 알림 창 표시 여부
  const [errorMessage, setErrorMessage] = useState<string>('') // 에러 메세지 문자열
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false) // 에러 알림 창 표시 여부

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'AttractionDetailScreen'>>()
  const attractionInfo = route.params.attractionInfo
  const toastRef = useRef<Toast | null>(null)
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  // 상단 바 뒤로가기 버튼 클릭 이벤트 함수
  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  // 명소 사진 컴포넌트 리스트를 반환하는 useMemo
  const photoList = useMemo(() => {
    return attractionInfo.Photo.map((item, index) => (
      <ImageBackground
        style={{width: '100%', height: 250, justifyContent: 'flex-end'}}
        source={{uri: serverUrl + 'img/attraction/' + item.photo}}
        key={index.toString()}>
        <Text style={[styles.idText]}>{item.value}</Text>
      </ImageBackground>
    ))
  }, [attractionInfo])

  // 명소의 위치를 좌표 타입으로 반환하는 useMemo
  const location = useMemo(() => {
    let temp: Coord = {
      latitude: attractionInfo.Lat,
      longitude: attractionInfo.Long
    }
    return temp
  }, [attractionInfo])

  // 명소의 위치를 마커 컴포넌트를 반환하는 useMemo
  const marker = useMemo(
    () => (
      <Marker
        coordinate={{
          latitude: attractionInfo.Lat,
          longitude: attractionInfo.Long
        }}
        width={30}
        height={40}
        image={require('../assets/images/marker.png')}
      />
    ),
    [attractionInfo]
  )

  // 명소의 편의 시설 컴포넌트 리스트를 반환하는 useMemo
  const supportList = useMemo(() => {
    return attractionInfo.Support.map((item, index) => (
      <AttractionSupport name={item.support} key={index.toString()} />
    ))
  }, [attractionInfo])

  // 명소의 평균 별점을 반환하는 useMemo
  const starScore = useMemo(() => {
    let sum = 0
    if (reviewList.length == 0) {
      // 평균이 없을 경우 0.0을 반환
      return sum.toFixed(1)
    }
    for (let i = 0; i < reviewList.length; i++) {
      sum += reviewList[i].Star
    }
    return (sum / reviewList.length).toFixed(1)
  }, [reviewList])

  // 명소 후기 컴포넌트 리스트를 반환하는 useMemo
  const reviewComponent = useMemo(() => {
    return reviewList.map((item, index) => (
      <View
        style={{width: '100%', paddingHorizontal: 20, backgroundColor: 'white'}}
        key={index.toString()}>
        <ReviewComponentPlusIcon
          reviewInfo={item}
          iconPress={() => {
            setReportTarget(item.Index_num)
            if (item.WriterID === user.ID) {
              setShowSelectDeleteBar(true)
            } else {
              setShowSelectBar(true)
            }
          }}
        />
      </View>
    ))
  }, [reviewList, user])

  // 신고 여부 확인 알림창 확인 버튼 클릭 이벤트 함수
  const reportAlertPress = useCallback(() => {
    let data = new FormData()
    data.append('Target', 'Review')
    data.append('TargetID', reportTarget)
    post('InsertReport.php', data)
    setShowReportAlert(false)
    setErrorMessage('신고가 접수 되었습니다.')
    setShowErrorAlert(true)
  }, [reportTarget])

  // 삭제 버튼 클릭 이벤트 함수
  const deleteReview = useCallback(() => {
    let data = new FormData()
    data.append('Index_num', reportTarget)
    post('DeleteAttractionReview.php', data)
      .then(() => {
        setErrorMessage('삭제가 완료되었습니다.')
        setShowErrorAlert(true)
      })
      .catch((e) => {
        setErrorMessage('리뷰 삭제 진행에 오류가 발생했습니다.')
        setShowErrorAlert(true)
      })
  }, [reportTarget])

  // 명소 후기 리스트를 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('Target', attractionInfo.Index_num)
    post('GetAttractionReviewList.php', data)
      .then((attractionReviewListInfo) => attractionReviewListInfo.json())
      .then((attractionReviewListInfoObject) => {
        const {GetAttractionReviewList} = attractionReviewListInfoObject
        setReviewList(GetAttractionReviewList)
      })
      .then(() => setReviewLoading(false))
  }, [attractionInfo])

  return (
    <>
      {/* 로딩중일때 */}
      {reviewLoading && <Loading />}
      {/* 로딩중이 아닐때 */}
      {!reviewLoading && (
        <SafeAreaView style={[styles.view]}>
          {/* 화면 상단 바 */}
          <NavigationHeader
            viewStyle={{backgroundColor: 'white'}}
            Left={() => (
              <IconNative
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
          />
          <View style={{flex: 1, backgroundColor: '#F8F8FA'}}>
            <ScrollView
              showsVerticalScrollIndicator={false}
              contentContainerStyle={{paddingBottom: 10}}>
              {/* 명소 사진 리스트 */}
              <Swiper
                horizontal
                width={Dimensions.get('window').width}
                height={250}
                containerStyle={{marginTop: 10}}
                autoplay={false}
                dot={
                  <View
                    style={[
                      styles.swiperdot,
                      {
                        width:
                          (Dimensions.get('window').width - 150) /
                          attractionInfo.Photo.length
                      }
                    ]}
                  />
                }
                activeDot={
                  <View
                    style={[
                      styles.activedot,
                      {
                        width:
                          (Dimensions.get('window').width - 150) /
                          attractionInfo.Photo.length
                      }
                    ]}
                  />
                }>
                {photoList}
              </Swiper>
              <View style={[styles.normalView]}>
                {/* 명소 이름 */}
                <Text style={{fontSize: 18, fontWeight: 'bold'}}>
                  {attractionInfo.Name}
                </Text>
                {/* 명소 설명 */}
                <Text style={{fontSize: 13, fontWeight: 'bold', marginTop: 5}}>
                  {attractionInfo.Subtitle}
                </Text>
              </View>
              {/* 명소 위치 지도 */}
              <NaverMapView
                style={[styles.mapView]}
                center={{...location, zoom: 16}}
                useTextureView={true}
                rotateGesturesEnabled={false}>
                {marker}
              </NaverMapView>
              {attractionInfo.Kind !== 'drive' && (
                <View style={[styles.normalView]}>
                  <View style={{flexDirection: 'row', alignItems: 'center'}}>
                    {/* 명소 주소 */}
                    <Text style={{fontSize: 13, marginRight: 15}}>
                      주소: {attractionInfo.Address}
                    </Text>
                    {/* 명소 주소 복사 아이콘 */}
                    <Icon
                      size={20}
                      name="content-copy"
                      onPress={() => {
                        Clipboard.setString(attractionInfo.Address)
                        toastRef.current?.show('주소가 복사되었습니다.')
                      }}
                    />
                  </View>
                  {attractionInfo.PhoneNumber.length != 0 && (
                    // 명소 전화번호
                    <View style={{marginTop: 5, flexDirection: 'row'}}>
                      <Text style={{fontSize: 13}}>전화번호 :</Text>
                      <Text
                        style={{fontSize: 13, textDecorationLine: 'underline'}}
                        onPress={() => {
                          Linking.openURL('tel:' + attractionInfo.PhoneNumber)
                        }}>
                        {attractionInfo.PhoneNumber}
                      </Text>
                    </View>
                  )}
                  {/* 명소 영업시간 */}
                  <Text style={{fontSize: 13, marginTop: 5, lineHeight: 20}}>
                    영업시간:{'\n'}
                    {attractionInfo.OpenHours}
                  </Text>
                </View>
              )}
              {(attractionInfo.Kind === 'cafe' ||
                attractionInfo.Kind === 'rest') && (
                // 명소 메뉴
                <View style={[styles.normalView]}>
                  <Text style={{fontSize: 13, lineHeight: 20}}>
                    메뉴{'\n'}
                    {'\n'}
                    {attractionInfo.Menu}
                  </Text>
                </View>
              )}
              {/* 명소 편의 시설 */}
              <View style={[styles.normalView]}>
                <Text style={{fontSize: 13}}>편의</Text>
                <View style={{flexDirection: 'row', flexWrap: 'wrap'}}>
                  {supportList}
                </View>
              </View>
              <View
                style={{
                  marginTop: 10,
                  flexDirection: 'row',
                  paddingHorizontal: 20,
                  alignItems: 'center'
                }}>
                <View style={{flex: 1, justifyContent: 'space-between'}}>
                  <View style={{flexDirection: 'row', alignItems: 'center'}}>
                    <FastImage
                      style={{width: 30, height: 30}}
                      source={require('../assets/images/star_fill.png')}
                    />
                    {/* 명소 평균 별점 */}
                    <Text style={{marginLeft: 10, fontSize: 14}}>
                      {starScore} / 5.0
                    </Text>
                  </View>
                  {/* 명소 후기 갯수 */}
                  <Text style={{fontSize: 12, color: '#767676', marginTop: 5}}>
                    리뷰 {reviewList.length}
                  </Text>
                </View>
                {/* 명소 후기 작성 아이콘 */}
                {user.ID !== 'guest' && (
                  <IconNative
                    size={45}
                    source={require('../assets/images/modify_background.png')}
                    onPress={() =>
                      navigation.navigate('WriteAttractionReview', {
                        Target: attractionInfo.Index_num
                      })
                    }
                  />
                )}
              </View>
              {/* 명소 후기가 없을 때 */}
              {reviewList.length == 0 && (
                <View style={[styles.no_review]}>
                  <FastImage
                    style={{width: '70%', height: 300}}
                    source={require('../assets/images/no_review.png')}
                    resizeMode="contain"
                  />
                </View>
              )}
              {/* 명소 후기가 존재할 때 */}
              {reviewList.length != 0 && (
                <View style={{marginTop: 20}}>{reviewComponent}</View>
              )}
            </ScrollView>
          </View>
          {/* 토스트 메세지 */}
          <Toast ref={toastRef} positionValue={100} />
        </SafeAreaView>
      )}
      {/* 명소 후기 신고하기 하단 바 */}
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
      {/* 명소 후기 삭제하기 하단 바 */}
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
      {/* 신고 여부 알림창 */}
      {showReportAlert && (
        <AlertComponent
          cancelUse={true}
          message="신고하시겠습니까?"
          cancelPress={() => setShowReportAlert(false)}
          okPress={reportAlertPress}
        />
      )}
      {/* 오류 알림창 */}
      {showErrorAlert && (
        <AlertComponent
          cancelUse={false}
          message={errorMessage}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
    </>
  )
}
