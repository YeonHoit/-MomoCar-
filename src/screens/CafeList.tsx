import React, {useEffect, useState, useMemo} from 'react'
import {
  StyleSheet,
  View,
  Platform,
  PermissionsAndroid,
  ScrollView
} from 'react-native'
import Geolocation from 'react-native-geolocation-service'
import type {Attraction} from '../type'
import {post} from '../server'
import {Loading, AttractionMinimal, AlertComponent} from '../components'
import FastImage from 'react-native-fast-image'

const style = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: '#F8F8FA'
  }
})

export default function CafeList() {
  const [attractionList, setAttractionList] = useState<Attraction[]>([])
  const [attractionLoading, setAttractionLoading] = useState<boolean>(true)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)

  const attractionComponent = useMemo(() => {
    return attractionList.map((item, index) => (
      <View style={{marginTop: 10}} key={index.toString()}>
        <AttractionMinimal attractionInfo={item} showDetail={false} />
      </View>
    ))
  }, [attractionList])

  const getAttraction = (latitude: number, longitude: number) => {
    setAttractionList([])
    let data = new FormData()
    data.append('latitude', latitude)
    data.append('longitude', longitude)
    data.append('kind', 'cafe')
    post('GetAttractionListforKind.php', data)
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
    setAttractionList([])
    let data = new FormData()
    data.append('kind', 'cafe')
    post('GetAllAttractionListforKind.php', data)
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

  return (
    <>
      {attractionLoading && <Loading />}
      {!attractionLoading && (
        <View style={[style.view]}>
          {attractionList.length == 0 && (
            <View
              style={{
                flex: 1,
                alignItems: 'center',
                justifyContent: 'center'
              }}>
              <FastImage
                style={{width: '55%', height: 150}}
                source={require('../assets/images/waiting.png')}
                resizeMode="contain"
              />
            </View>
          )}
          {attractionList.length != 0 && (
            <ScrollView
              // contentContainerStyle={{paddingHorizontal: 10}}
              showsVerticalScrollIndicator={false}>
              {attractionComponent}
            </ScrollView>
          )}
        </View>
      )}
      {showAlert && (
        <AlertComponent
          cancelUse={false}
          message={alertMessage}
          okPress={() => setShowAlert(false)}
        />
      )}
    </>
  )
}
