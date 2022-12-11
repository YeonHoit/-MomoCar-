import React, {useCallback, useState, useEffect} from 'react'
import {StyleSheet, Text, View, ScrollView, Dimensions} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  TouchableView,
  Icon,
  AlertComponent,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import {post} from '../server'
import {useDispatch, useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import type {LayoutChangeEvent} from 'react-native'
import * as RNFS from 'react-native-fs'
import type {UserJson} from '../type'
import type {ParamList} from '../navigator/MainNavigator'

const screenWidth = Dimensions.get('window').width

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  text: {
    fontSize: 14,
    textAlignVertical: 'center',
    flex: 1,
    color: 'black'
  },
  touchableView: {
    padding: 10,
    backgroundColor: 'white',
    borderBottomWidth: 1,
    width: screenWidth * 0.9,
    borderColor: '#DBDBDB',
    marginBottom: 10,
    flexDirection: 'row'
  },
  titleText: {
    fontSize: 24,
    color: '#191919',
    fontWeight: '800',
    marginVertical: 70
  }
})

export default function SetYearType() {
  const [yearTypeList, setYearTypeList] = useState<string[]>([])
  const [textMarginLeft, setTextMarginLeft] = useState<number>(0)
  const [listCheck, setListCheck] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'SetYearType'>>()
  const carType = route.params.carType
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {ID} = user
  const dispatch = useDispatch()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const yearType_onClick = useCallback(
    (yearType_select: string) => {
      let data = new FormData()
      data.append('ID', ID)
      data.append('target', 'UserCar')
      data.append('value', yearType_select)
      post('UpdateUserInfo.php', data)
        // .then(() => {
        //   dispatch(Ur.setUserAction({...user, UserCar: yearType_select}))
        // })
        .then(() => {
          let data = new FormData()
          data.append('ID', ID)
          data.append('target', 'UserCarType')
          data.append('value', carType)
          post('UpdateUserInfo.php', data)
            .then(() => {
              dispatch(
                Ur.setUserAction({
                  ...user,
                  UserCar: yearType_select,
                  UserCarType: carType
                })
              )
              // navigation.navigate('DrawerNavigator')
            })
            .then(() => {
              navigation.reset({routes: [{name: 'DrawerNavigator'}]})
            })
            .catch((e) => {
              setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
              setShowAlert(true)
            })
        })
        .catch((e) => {
          setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
          setShowAlert(true)
        })
    },
    [user]
  )

  useEffect(() => {
    let data = new FormData()
    data.append('CarType', carType)
    post('GetYearType.php', data)
      .then((YearTypeInfo) => YearTypeInfo.json())
      .then((YearTypeInfoObject) => {
        const {GetYearType} = YearTypeInfoObject
        setYearTypeList(GetYearType)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setAlertMessage('연식 정보를 불러오지 못했습니다.')
        setShowAlert(true)
        setLoading(false)
      })
  }, [carType])

  const onLayout = useCallback((e: LayoutChangeEvent) => {
    const {layout} = e.nativeEvent
    setTextMarginLeft(layout.x)
    setListCheck(true)
  }, [])

  const children = yearTypeList.map((yeartype, index) => {
    if (index == 0) {
      return (
        <TouchableView
          viewStyle={[styles.touchableView]}
          onPress={() => yearType_onClick(yeartype)}
          key={index.toString()}>
          <Text style={[styles.text]} onLayout={onLayout}>
            {yeartype}
          </Text>
          <Icon
            source={require('../assets/images/chevron_right.png')}
            size={12}
          />
        </TouchableView>
      )
    } else {
      return (
        <TouchableView
          viewStyle={[styles.touchableView]}
          onPress={() => yearType_onClick(yeartype)}
          key={index.toString()}>
          <Text style={[styles.text]}>{yeartype}</Text>
          <Icon
            source={require('../assets/images/chevron_right.png')}
            size={12}
          />
        </TouchableView>
      )
    }
  })

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
          />
          <ScrollView showsVerticalScrollIndicator={false}>
            {listCheck && (
              <Text
                style={[styles.titleText, {marginLeft: textMarginLeft + 30}]}>
                차량의 세부 정보와{'\n'}연식을 선택해주세요
              </Text>
            )}
            <View style={[{alignItems: 'center'}]}>{children}</View>
          </ScrollView>
        </SafeAreaView>
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
