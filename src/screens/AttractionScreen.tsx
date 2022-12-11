import React, {useCallback, useState, useMemo, useEffect} from 'react'
import {
  StyleSheet,
  View,
  ScrollView,
  Platform,
  PermissionsAndroid,
  Text
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  Loading,
  AlertComponent,
  AttractionMinimal
} from '../components'
import {useNavigation} from '@react-navigation/native'
import Geolocation from 'react-native-geolocation-service'
import type {Attraction} from '../type'
import {post} from '../server'
import FastImage from 'react-native-fast-image'
import AttractionNavigator from '../navigator/AttractionNavigator'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  topView: {
    width: '100%',
    backgroundColor: 'white',
    borderBottomLeftRadius: 20,
    borderBottomRightRadius: 20,
    alignItems: 'center',
    paddingHorizontal: 20,
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingBottom: 15
  }
})

export default function AttractionScreen() {
  const [selectKind, setSelectKind] = useState<string>('cafe')
  const [attractionList, setAttractionList] = useState<Attraction[]>([])
  const [attractionLoading, setAttractionLoading] = useState<boolean>(true)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)

  const navigation = useNavigation()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const cafeButton = useMemo(() => {
    if (selectKind === 'cafe') {
      return (
        <Icon size={53} source={require('../assets/images/cafe_fill.png')} />
      )
    }
    return (
      <Icon
        size={53}
        source={require('../assets/images/cafe_empty.png')}
        onPress={() => setSelectKind('cafe')}
      />
    )
  }, [selectKind])

  const restButton = useMemo(() => {
    if (selectKind === 'rest') {
      return (
        <Icon size={53} source={require('../assets/images/rest_fill.png')} />
      )
    }
    return (
      <Icon
        size={53}
        source={require('../assets/images/rest_empty.png')}
        onPress={() => setSelectKind('rest')}
      />
    )
  }, [selectKind])

  const attractionButton = useMemo(() => {
    if (selectKind === 'attraction') {
      return (
        <Icon
          size={53}
          source={require('../assets/images/attraction_fill.png')}
        />
      )
    }
    return (
      <Icon
        size={53}
        source={require('../assets/images/attraction_empty.png')}
        onPress={() => setSelectKind('attraction')}
      />
    )
  }, [selectKind])

  const driveButton = useMemo(() => {
    if (selectKind === 'drive') {
      return (
        <Icon size={53} source={require('../assets/images/drive_fill.png')} />
      )
    }
    return (
      <Icon
        size={53}
        source={require('../assets/images/drive_empty.png')}
        onPress={() => setSelectKind('drive')}
      />
    )
  }, [selectKind])

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
    data.append('kind', selectKind)
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
    data.append('kind', selectKind)
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
  }, [selectKind])

  return (
    <>
      {attractionLoading && <Loading />}
      {!attractionLoading && (
        <SafeAreaView style={[styles.view]}>
          <NavigationHeader
            viewStyle={{backgroundColor: 'white'}}
            Left={() => (
              <Icon
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
          />
          {/* <View style={{flex: 1, backgroundColor: '#F8F8FA'}}>
            <View style={[styles.topView]}>
              {cafeButton}
              {restButton}
              {attractionButton}
              {driveButton}
            </View>
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
          </View> */}
          <AttractionNavigator />
        </SafeAreaView>
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
