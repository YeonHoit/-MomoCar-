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

// ?????? ????????? ?????? ??????
export default function AttractionDetailScreen() {
  const [reviewList, setReviewList] = useState<AttractionReview[]>([]) // ?????? ?????? ????????? ??????
  const [reviewLoading, setReviewLoading] = useState<boolean>(true) // ?????? ?????? ??????
  const [showSelectBar, setShowSelectBar] = useState<boolean>(false) // ?????? ??? ?????? ??????
  const [showSelectDeleteBar, setShowSelectDeleteBar] = useState<boolean>(false) // ?????? ??? ?????? ??????
  const [reportTarget, setReportTarget] = useState<number>(-1) // ?????? ??? ?????? ??????
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false) // ?????? ?????? ?????? ??? ?????? ??????
  const [errorMessage, setErrorMessage] = useState<string>('') // ?????? ????????? ?????????
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false) // ?????? ?????? ??? ?????? ??????

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'AttractionDetailScreen'>>()
  const attractionInfo = route.params.attractionInfo
  const toastRef = useRef<Toast | null>(null)
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  // ?????? ??? ???????????? ?????? ?????? ????????? ??????
  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  // ?????? ?????? ???????????? ???????????? ???????????? useMemo
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

  // ????????? ????????? ?????? ???????????? ???????????? useMemo
  const location = useMemo(() => {
    let temp: Coord = {
      latitude: attractionInfo.Lat,
      longitude: attractionInfo.Long
    }
    return temp
  }, [attractionInfo])

  // ????????? ????????? ?????? ??????????????? ???????????? useMemo
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

  // ????????? ?????? ?????? ???????????? ???????????? ???????????? useMemo
  const supportList = useMemo(() => {
    return attractionInfo.Support.map((item, index) => (
      <AttractionSupport name={item.support} key={index.toString()} />
    ))
  }, [attractionInfo])

  // ????????? ?????? ????????? ???????????? useMemo
  const starScore = useMemo(() => {
    let sum = 0
    if (reviewList.length == 0) {
      // ????????? ?????? ?????? 0.0??? ??????
      return sum.toFixed(1)
    }
    for (let i = 0; i < reviewList.length; i++) {
      sum += reviewList[i].Star
    }
    return (sum / reviewList.length).toFixed(1)
  }, [reviewList])

  // ?????? ?????? ???????????? ???????????? ???????????? useMemo
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

  // ?????? ?????? ?????? ????????? ?????? ?????? ?????? ????????? ??????
  const reportAlertPress = useCallback(() => {
    let data = new FormData()
    data.append('Target', 'Review')
    data.append('TargetID', reportTarget)
    post('InsertReport.php', data)
    setShowReportAlert(false)
    setErrorMessage('????????? ?????? ???????????????.')
    setShowErrorAlert(true)
  }, [reportTarget])

  // ?????? ?????? ?????? ????????? ??????
  const deleteReview = useCallback(() => {
    let data = new FormData()
    data.append('Index_num', reportTarget)
    post('DeleteAttractionReview.php', data)
      .then(() => {
        setErrorMessage('????????? ?????????????????????.')
        setShowErrorAlert(true)
      })
      .catch((e) => {
        setErrorMessage('?????? ?????? ????????? ????????? ??????????????????.')
        setShowErrorAlert(true)
      })
  }, [reportTarget])

  // ?????? ?????? ???????????? ???????????? useEffect
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
      {/* ??????????????? */}
      {reviewLoading && <Loading />}
      {/* ???????????? ????????? */}
      {!reviewLoading && (
        <SafeAreaView style={[styles.view]}>
          {/* ?????? ?????? ??? */}
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
              {/* ?????? ?????? ????????? */}
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
                {/* ?????? ?????? */}
                <Text style={{fontSize: 18, fontWeight: 'bold'}}>
                  {attractionInfo.Name}
                </Text>
                {/* ?????? ?????? */}
                <Text style={{fontSize: 13, fontWeight: 'bold', marginTop: 5}}>
                  {attractionInfo.Subtitle}
                </Text>
              </View>
              {/* ?????? ?????? ?????? */}
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
                    {/* ?????? ?????? */}
                    <Text style={{fontSize: 13, marginRight: 15}}>
                      ??????: {attractionInfo.Address}
                    </Text>
                    {/* ?????? ?????? ?????? ????????? */}
                    <Icon
                      size={20}
                      name="content-copy"
                      onPress={() => {
                        Clipboard.setString(attractionInfo.Address)
                        toastRef.current?.show('????????? ?????????????????????.')
                      }}
                    />
                  </View>
                  {attractionInfo.PhoneNumber.length != 0 && (
                    // ?????? ????????????
                    <View style={{marginTop: 5, flexDirection: 'row'}}>
                      <Text style={{fontSize: 13}}>???????????? :</Text>
                      <Text
                        style={{fontSize: 13, textDecorationLine: 'underline'}}
                        onPress={() => {
                          Linking.openURL('tel:' + attractionInfo.PhoneNumber)
                        }}>
                        {attractionInfo.PhoneNumber}
                      </Text>
                    </View>
                  )}
                  {/* ?????? ???????????? */}
                  <Text style={{fontSize: 13, marginTop: 5, lineHeight: 20}}>
                    ????????????:{'\n'}
                    {attractionInfo.OpenHours}
                  </Text>
                </View>
              )}
              {(attractionInfo.Kind === 'cafe' ||
                attractionInfo.Kind === 'rest') && (
                // ?????? ??????
                <View style={[styles.normalView]}>
                  <Text style={{fontSize: 13, lineHeight: 20}}>
                    ??????{'\n'}
                    {'\n'}
                    {attractionInfo.Menu}
                  </Text>
                </View>
              )}
              {/* ?????? ?????? ?????? */}
              <View style={[styles.normalView]}>
                <Text style={{fontSize: 13}}>??????</Text>
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
                    {/* ?????? ?????? ?????? */}
                    <Text style={{marginLeft: 10, fontSize: 14}}>
                      {starScore} / 5.0
                    </Text>
                  </View>
                  {/* ?????? ?????? ?????? */}
                  <Text style={{fontSize: 12, color: '#767676', marginTop: 5}}>
                    ?????? {reviewList.length}
                  </Text>
                </View>
                {/* ?????? ?????? ?????? ????????? */}
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
              {/* ?????? ????????? ?????? ??? */}
              {reviewList.length == 0 && (
                <View style={[styles.no_review]}>
                  <FastImage
                    style={{width: '70%', height: 300}}
                    source={require('../assets/images/no_review.png')}
                    resizeMode="contain"
                  />
                </View>
              )}
              {/* ?????? ????????? ????????? ??? */}
              {reviewList.length != 0 && (
                <View style={{marginTop: 20}}>{reviewComponent}</View>
              )}
            </ScrollView>
          </View>
          {/* ????????? ????????? */}
          <Toast ref={toastRef} positionValue={100} />
        </SafeAreaView>
      )}
      {/* ?????? ?????? ???????????? ?????? ??? */}
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
              <Text style={[styles.selectItemText]}>????????????</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {/* ?????? ?????? ???????????? ?????? ??? */}
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
              <Text style={[styles.selectItemText]}>????????????</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {/* ?????? ?????? ????????? */}
      {showReportAlert && (
        <AlertComponent
          cancelUse={true}
          message="?????????????????????????"
          cancelPress={() => setShowReportAlert(false)}
          okPress={reportAlertPress}
        />
      )}
      {/* ?????? ????????? */}
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
