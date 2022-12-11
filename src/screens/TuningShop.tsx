import React, {useCallback, useState, useEffect, useMemo, useRef} from 'react'
import {
  StyleSheet,
  Platform,
  View,
  Dimensions,
  Text,
  PermissionsAndroid,
  Linking
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  TouchableView,
  AlertComponent,
  Loading
} from '../components'
import {useNavigation, DrawerActions} from '@react-navigation/native'
import NaverMapView, {Coord, Marker} from 'react-native-nmap'
import Geolocation from 'react-native-geolocation-service'
import {post_without_data, serverUrl} from '../server'
import type {TuningShopType} from '../type'
import FastImage from 'react-native-fast-image'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'

let {width} = Dimensions.get('window')

const styles = StyleSheet.create({
  safeAreaView: {
    flex: 1,
    backgroundColor: 'white'
  },
  mapView: {
    flex: 1
  },
  markerInfo: {
    // height: 250,
    backgroundColor: 'white',
    position: 'absolute',
    bottom: 50,
    left: 10,
    right: 10,
    borderRadius: 18
  },
  shopImage: {
    width: (width - 40) / 2,
    height: 200
  },
  text1: {
    color: 'black',
    fontSize: 13
  },
  text2: {
    color: '#767676',
    fontSize: 12,
    marginTop: 5,
    marginBottom: 15,
    maxWidth: (width - 45) / 2
  },
  buttonView: {
    height: 50,
    borderColor: '#DBDBDB',
    // borderTopWidth: 1,
    // borderBottomWidth: 1,
    flexDirection: 'row'
  },
  button: {
    alignItems: 'center',
    justifyContent: 'center',
    flex: 1
  }
})

export default function TuningShop() {
  const navigation = useNavigation()
  const [tuningShopList, setTuningShopList] = useState<TuningShopType[]>([])
  const [showMarkerInfo, setShowMarkerInfo] = useState<boolean>(false)
  const [selectedMarker, setSelectedMarker] = useState<TuningShopType>({
    Index_num: -1,
    Name: '',
    Address: '',
    Photo: '',
    PhoneNumber: '',
    lat: 0,
    long: 0
  })
  const [currentLocation, setCurrentLocation] = useState<Coord>({
    latitude: 35.12157,
    longitude: 129.10341
  })
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)

  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const drawerOpen = useCallback(() => {
    navigation.dispatch(DrawerActions.openDrawer())
  }, [])

  const markerClick = useCallback((tuningshop: TuningShopType) => {
    setSelectedMarker(tuningshop)
    setShowMarkerInfo(true)
  }, [])

  const marker_list = useMemo(
    () =>
      tuningShopList.map((tuningShop, index) => (
        <Marker
          coordinate={{latitude: tuningShop.lat, longitude: tuningShop.long}}
          width={30}
          height={40}
          key={index.toString()}
          onClick={() => markerClick(tuningShop)}
          image={require('../assets/images/marker.png')}
        />
      )),
    [tuningShopList]
  )

  useEffect(() => {
    if (Platform.OS === 'ios') {
      Geolocation.requestAuthorization('always').then((grant) => {
        if (grant === 'granted') {
          Geolocation.getCurrentPosition(
            (position) => {
              setCurrentLocation({
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
              })
            },
            (error) => {
              setAlertMessage(error.code.toString() + error.message)
              setShowAlert(true)
            }
          )
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
              setCurrentLocation({
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
              })
            },
            (error) => {
              setAlertMessage(error.code.toString() + error.message)
              setShowAlert(true)
            }
          )
        }
      })
    }
    post_without_data('GetTuningShopInfo.php')
      .then((TuningShopInfo) => TuningShopInfo.json())
      .then((TuningShopInfoObject) => {
        const {GetTuningShopInfo} = TuningShopInfoObject
        for (let s of GetTuningShopInfo) {
          const {Index_num, Name, Address, Photo, PhoneNumber, lat, long} = s
          setTuningShopList((tuningShopList) => [
            ...tuningShopList,
            {
              Index_num,
              Name,
              Address,
              Photo,
              PhoneNumber,
              lat: parseFloat(lat),
              long: parseFloat(long)
            }
          ])
        }
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setAlertMessage('튜닝샵 정보를 불러오지 못했습니다.')
        setShowAlert(true)
        setLoading(false)
      })
  }, [])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
        <SafeAreaView style={[styles.safeAreaView]}>
          <NavigationHeader
            viewStyle={{backgroundColor: 'white'}}
            Left={() => (
              <Text style={{fontSize: 16, fontWeight: 'bold'}}>튜닝샵</Text>
            )}
          />
          <NaverMapView
            style={[styles.mapView]}
            center={{...currentLocation, zoom: 12}}
            onMapClick={() => setShowMarkerInfo(false)}
            useTextureView={true}
            rotateGesturesEnabled={false}
            showsMyLocationButton={true}>
            {marker_list}
          </NaverMapView>
        </SafeAreaView>
      )}
      {showMarkerInfo && (
        <View style={[styles.markerInfo, {width: width - 20}]}>
          <View
            style={{
              flexDirection: 'row',
              paddingHorizontal: 10,
              paddingTop: 10,
              flex: 1
            }}>
            <View
              style={{
                alignItems: 'center',
                justifyContent: 'center'
              }}>
              <FastImage
                style={[styles.shopImage]}
                source={{
                  uri: serverUrl + 'img/' + selectedMarker.Photo
                }}
                resizeMode="stretch"
              />
            </View>
            <View style={{marginLeft: 15}}>
              <Text style={[styles.text1]}>업체이름</Text>
              <Text style={[styles.text2]}>{selectedMarker.Name}</Text>
              <Text style={[styles.text1]}>주소</Text>
              <Text style={[styles.text2]}>{selectedMarker.Address}</Text>
              <Text style={[styles.text1]}>전화번호</Text>
              <Text
                style={[styles.text2, {textDecorationLine: 'underline'}]}
                onPress={() => {
                  Linking.openURL('tel:' + selectedMarker.PhoneNumber)
                }}>
                {selectedMarker.PhoneNumber}
              </Text>
            </View>
          </View>
          <View style={[styles.buttonView]}>
            <TouchableView
              style={[styles.button]}
              viewStyle={{
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'center'
              }}>
              <FastImage
                source={require('../assets/images/question.png')}
                style={{width: 20, height: 20}}
              />
              <Text style={{fontSize: 14, paddingLeft: 5}}>1대1 문의</Text>
            </TouchableView>
            <TouchableView
              style={[styles.button]}
              viewStyle={{
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'center'
              }}>
              <FastImage
                source={require('../assets/images/reserve.png')}
                style={{width: 20, height: 20}}
              />
              <Text style={{fontSize: 14, paddingLeft: 5}}>예약하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
    </>
  )
}
