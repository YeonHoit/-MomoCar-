import React, {useCallback, useState, useEffect, useMemo} from 'react'
import {
  StyleSheet,
  Text,
  View,
  ScrollView,
  Dimensions,
  Platform,
  PermissionsAndroid,
  ImageBackground
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  AlertComponent,
  YoutubeSlider,
  Loading,
  AttractionMinimal,
  TouchableView
} from '../components'
import {useNavigation, DrawerActions} from '@react-navigation/native'
import {post_without_data, post} from '../server'
import type {YoutubeType, Attraction, UserJson} from '../type'
import Color from 'color'
import FastImage from 'react-native-fast-image'
import Geolocation from 'react-native-geolocation-service'
import Swiper from 'react-native-swiper'
import {UnityModule} from '@asmadsen/react-native-unity-view'
import {readFromStorage, writeToStorage} from '../utils'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import messaging from '@react-native-firebase/messaging'
import Orientation from 'react-native-orientation-locker'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  content: {
    alignItems: 'center'
  },
  text: {
    fontSize: 20,
    textAlign: 'center',
    textAlignVertical: 'center',
    width: 300,
    height: 45
  },
  imageSliderView: {
    width: '100%',
    alignItems: 'center',
    justifyContent: 'center'
  },
  unityButton: {
    width: 200,
    height: 40,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#E6135A',
    marginTop: 5,
    marginBottom: 10,
    borderRadius: 10
  },
  topView: {
    width: '100%',
    backgroundColor: 'white',
    borderBottomLeftRadius: 20,
    borderBottomRightRadius: 20,
    alignItems: 'center',
    marginBottom: 20
  },
  youtubeView: {
    width: Dimensions.get('window').width,
    marginTop: 15
    // marginBottom: 10
  },
  eventTotalView: {
    width: Dimensions.get('window').width,
    // marginTop: 5,
    marginBottom: 100
  },
  eventView: {
    backgroundColor: 'white',
    width: Dimensions.get('window').width - 20,
    height: 200,
    marginTop: 10,
    borderRadius: 20,
    padding: 10,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: Color('#DBDBDB').lighten(0.1).toString(),
    marginLeft: 10,
    justifyContent: 'center'
  },
  swiperdot: {
    backgroundColor: '#EDEDED',
    height: 2,
    borderRadius: 4,
    marginLeft: 3,
    marginRight: 3,
    marginTop: 3,
    marginBottom: -3
  },
  activedot: {
    backgroundColor: '#E6135A',
    height: 2,
    borderRadius: 4,
    marginLeft: 3,
    marginRight: 3,
    marginTop: 3,
    marginBottom: -3
  },
  goUnityView: {
    // width: Dimensions.get('window').width - 20,
    width: Dimensions.get('window').width,
    height: 250,
    backgroundColor: 'white',
    // borderRadius: 20,
    padding: 10
    // marginVertical: 20
    // marginTop: 5
    // marginBottom: 4
  },
  goUnityImage: {
    width: Dimensions.get('window').width - 20,
    height: 230,
    borderRadius: 8,
    overflow: 'hidden'
  },
  goUnityTitleText: {
    color: 'white',
    fontSize: 20,
    marginLeft: 15,
    fontWeight: 'bold',
    textShadowColor: 'rgba(0, 0, 0, 0.9)',
    textShadowOffset: {width: -1, height: 1},
    textShadowRadius: 10
  },
  goUnitySmallText: {
    color: 'white',
    marginRight: 15,
    marginBottom: 15,
    fontSize: 12,
    fontWeight: 'bold',
    textShadowColor: 'rgba(0, 0, 0, 0.9)',
    textShadowOffset: {width: -1, height: 1},
    textShadowRadius: 10
  }
})

let temp: Attraction = {
  Index_num: 0,
  Name: '',
  Kind: '',
  Photo: [],
  Subtitle: '',
  Lat: 0,
  Long: 0,
  Address: '',
  PhoneNumber: '',
  OpenHours: '',
  Menu: '',
  Support: []
}

export default function Home() {
  const [youtubeInfoList, setYoutubeInfoList] = useState<YoutubeType[]>([])
  const [attractionList, setAttractionList] = useState<Attraction[]>([temp])
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [youtubeLoading, setYoutubeLoading] = useState<boolean>(true)
  const [attractionLoading, setAttractionLoading] = useState<boolean>(true)
  const [showUnityAlert, setShowUnityAlert] = useState<boolean>(false)

  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const loading = useMemo(() => {
    if (!youtubeLoading && !attractionLoading) {
      return false
    }
    return true
  }, [youtubeLoading, attractionLoading])

  const drawerOpen = useCallback(() => {
    navigation.dispatch(DrawerActions.openDrawer())
  }, [])

  const eventPress = useCallback(() => {
    setAlertMessage('준비중 입니다.')
    setShowAlert(true)
  }, [])

  const getAttraction = (latitude: number, longitude: number) => {
    setAttractionList([temp])
    let data = new FormData()
    data.append('latitude', latitude)
    data.append('longitude', longitude)
    post('GetAttractionList.php', data)
      .then((AttractionListInfo) => AttractionListInfo.json())
      .then((AttractionListInfoObject) => {
        const {GetAttractionList} = AttractionListInfoObject
        for (let s of GetAttractionList) {
          //prettier-ignore
          const {Index_num, Name, Kind, Photo, Subtitle, Lat, Long, Address, PhoneNumber, OpenHours, Menu, Support} = s
          setAttractionList((list) => [
            ...list,
            {
              Index_num,
              Name,
              Kind,
              Photo: JSON.parse(Photo),
              Subtitle,
              Lat: parseFloat(Lat),
              Long: parseFloat(Long),
              Address,
              PhoneNumber,
              OpenHours,
              Menu,
              Support: JSON.parse(Support)
            }
          ])
        }
      })
      .then(() => setAttractionLoading(false))
      .catch(() => {
        setAttractionLoading(false)
      })
  }

  const getAllAttraction = () => {
    setAttractionList([temp])
    post_without_data('GetAllAttractionList.php')
      .then((AttractionListInfo) => AttractionListInfo.json())
      .then((AttractionListInfoObject) => {
        const {GetAttractionList} = AttractionListInfoObject
        for (let s of GetAttractionList) {
          //prettier-ignore
          const {Index_num, Name, Kind, Photo, Subtitle, Lat, Long, Address, PhoneNumber, OpenHours, Menu, Support} = s
          setAttractionList((list) => [
            ...list,
            {
              Index_num,
              Name,
              Kind,
              Photo: JSON.parse(Photo),
              Subtitle,
              Lat: parseFloat(Lat),
              Long: parseFloat(Long),
              Address,
              PhoneNumber,
              OpenHours,
              Menu,
              Support: JSON.parse(Support)
            }
          ])
        }
      })
      .then(() => setAttractionLoading(false))
      .catch(() => {
        setAttractionLoading(false)
      })
  }

  const requestUserPermission = async () => {
    await messaging().requestPermission()
  }

  useEffect(() => {
    requestUserPermission()
  }, [])

  useEffect(() => {
    post_without_data('GetYoutubeInfo.php')
      .then((YoutubeInfo) => YoutubeInfo.json())
      .then((YoutubeInfoObject) => {
        const {GetYoutubeInfo} = YoutubeInfoObject
        setYoutubeInfoList(GetYoutubeInfo)
      })
      .then(() => {
        setYoutubeLoading(false)
      })
      .catch((e) => {
        setAlertMessage('유튜브 영상 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setYoutubeLoading(false)
      })
  }, [])

  useEffect(() => {
    if (Platform.OS === 'ios') {
      Geolocation.requestAuthorization('always').then((grant) => {
        if (grant === 'granted') {
          Geolocation.getCurrentPosition(
            (position) => {
              getAttraction(position.coords.latitude, position.coords.longitude)
            },
            (error) => {
              setAlertMessage(error.code.toString() + error.message)
              setShowAlert(true)
            },
            {
              enableHighAccuracy: true,
              timeout: 3600,
              maximumAge: 3600
            }
          )
        } else {
          getAllAttraction()
        }
      })
    }
    if (Platform.OS === 'android') {
      PermissionsAndroid.request(
        PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION
      ).then((grant) => {
        if (grant === 'granted') {
          Geolocation.getCurrentPosition(
            (position) => {
              getAttraction(position.coords.latitude, position.coords.longitude)
            },
            (error) => {
              setAlertMessage(error.code.toString() + error.message)
              setShowAlert(true)
            }
          )
        } else {
          getAllAttraction()
        }
      })
    }
  }, [])

  useEffect(() => {
    let temp: UserJson
    readFromStorage('userjson').then((value) => {
      if (value.length > 0) {
        let userjson: UserJson = JSON.parse(value)
        temp = {...userjson, ID: user.ID, UserCar: user.UserCar}
        writeToStorage('userjson', JSON.stringify(temp))
      } else {
        temp = {
          ID: user.ID,
          UserCar: user.UserCar,
          quality: 'H',
          frame: 'H'
        }
        writeToStorage('userjson', JSON.stringify(temp))
      }
    })
  }, [user])

  useEffect(() => {
    let data = new FormData()
    data.append('ID', user.ID)
    post('UpdateConnectCount.php', data).catch((e) => {})
  }, [user])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
        <SafeAreaView style={[styles.view]}>
          <View style={[{flex: 1, backgroundColor: '#F8F8FA'}]}>
            <NavigationHeader
              viewStyle={[{backgroundColor: 'white'}]}
              Left={() => (
                <FastImage
                  style={{width: 49, height: 15}}
                  source={require('../assets/images/momocar_logo.png')}
                />
              )}
              Right={() => (
                <Icon
                  source={require('../assets/images/menu.png')}
                  size={30}
                  onPress={drawerOpen}
                />
              )}
            />
            <ScrollView contentContainerStyle={[styles.content]}>
              <TouchableView
                viewStyle={[styles.goUnityView]}
                onPress={() => setShowUnityAlert(true)}>
                <ImageBackground
                  style={[styles.goUnityImage]}
                  source={require('../assets/images/unity_test15.jpeg')}>
                  <Text style={[styles.goUnityTitleText, {marginTop: 15}]}>
                    관심 차종을 미리
                  </Text>
                  <Text style={[styles.goUnityTitleText]}>튜닝해보세요!</Text>
                  <View
                    style={{
                      alignItems: 'flex-end',
                      justifyContent: 'flex-end',
                      flex: 1
                    }}>
                    <Text style={[styles.goUnitySmallText]}>지금 시작하기</Text>
                  </View>
                </ImageBackground>
              </TouchableView>
              <Swiper
                horizontal={true}
                width={Dimensions.get('window').width}
                height={350}
                containerStyle={{borderRadius: 20}}
                autoplay
                autoplayTimeout={4}
                dot={
                  <View
                    style={[
                      styles.swiperdot,
                      {
                        width:
                          (Dimensions.get('window').width - 150) /
                          attractionList.length
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
                          attractionList.length
                      }
                    ]}
                  />
                }>
                {attractionList.map((item, index) => (
                  <AttractionMinimal
                    attractionInfo={item}
                    showDetail={true}
                    key={index.toString()}
                  />
                ))}
              </Swiper>
              <View style={[styles.youtubeView]}>
                <View
                  style={{
                    marginLeft: 20,
                    flexDirection: 'row',
                    marginRight: 20,
                    alignItems: 'flex-end'
                  }}>
                  <Text style={{fontSize: 16, fontWeight: 'bold', flex: 1}}>
                    자동차 추천 영상
                  </Text>
                  <Text
                    style={{color: '#999999', fontSize: 12}}
                    onPress={() => navigation.navigate('YoutubeRecommend')}>
                    더 알아보기 {'>'}
                  </Text>
                </View>
                <YoutubeSlider
                  youtubeViewInfo={youtubeInfoList}
                  youtubeWidth={Dimensions.get('window').width}
                />
              </View>
              <View style={[styles.eventTotalView]}>
                <View
                  style={{
                    marginLeft: 20,
                    flexDirection: 'row',
                    marginRight: 20,
                    alignItems: 'flex-end'
                  }}>
                  <Text style={{fontSize: 16, fontWeight: 'bold', flex: 1}}>
                    이벤트 혜택
                  </Text>
                  <Text
                    style={{color: '#999999', fontSize: 12}}
                    onPress={eventPress}>
                    더 알아보기 {'>'}
                  </Text>
                </View>
                <View style={[styles.eventView]}>
                  <FastImage
                    style={{width: '55%', height: 150}}
                    source={require('../assets/images/waiting.png')}
                    resizeMode="contain"
                  />
                </View>
              </View>
              <View style={{paddingHorizontal: 20, width: '100%'}}>
                <Text
                  style={{
                    marginTop: 30,
                    fontWeight: 'bold',
                    color: '#999999',
                    fontSize: 12
                  }}>
                  커스트(COUST)
                </Text>
                <Text style={{fontSize: 9, color: '#999999'}}>
                  대표: 이윤걸 {'\n'}
                  사업자등록번호: 313-07-03523{'\n'}
                  이메일: coust2121@gmail.com{'\n'}
                  주소: 부산광역시 남구 신선대로 428, 8동 3층 6호{'\n'}
                  호스팅 서비스: 카페24{'\n'}
                  {'\n'}
                  모모카에서 사용하는 모든 자동차 로고와 브랜드명, 디자인은 해당
                  자동차 회사의 소유이며, 모모카의 소유가 아닙니다.{'\n'}본
                  기능을 개인적인 용도로 사용하는 것을 권장 드리고, 상업적으로
                  이용하실 경우 발생하는 법적인 문제에 대해서는 모모카와 연관이
                  없음을 알려 드립니다.{'\n'}
                </Text>
              </View>
            </ScrollView>
          </View>
        </SafeAreaView>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
      {showUnityAlert && (
        <AlertComponent
          message="가상튜닝을 시작하시겠습니까?"
          cancelUse={true}
          cancelPress={() => setShowUnityAlert(false)}
          okPress={() => {
            setShowUnityAlert(false)
            if (Platform.OS === 'ios') {
              Orientation.unlockAllOrientations()
            }
            UnityModule.createUnity().then((value) => {
              navigation.navigate('Unity')
            })
          }}
        />
      )}
    </>
  )
}
