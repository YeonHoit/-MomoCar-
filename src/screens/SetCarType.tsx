import React, {useCallback, useEffect, useState} from 'react'
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
import type {LayoutChangeEvent} from 'react-native'
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

export default function SetCarType() {
  const [carTypeList, setCarTypeList] = useState<string[]>([])
  const [textMarginLeft, setTextMarginLeft] = useState<number>(0)
  const [listCheck, setListCheck] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)

  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'SetCarType'>>()
  const manufacturer = route.params.manufacturer

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const carType_onClick = useCallback((carType_select: string) => {
    navigation.navigate('SetYearType', {carType: carType_select})
  }, [])

  useEffect(() => {
    let data = new FormData()
    data.append('Manufacturer', manufacturer)
    post('GetCarType.php', data)
      .then((CarTypeInfo) => CarTypeInfo.json())
      .then((CarTypeInfoObject) => {
        const {GetCarType} = CarTypeInfoObject
        setCarTypeList(GetCarType)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setAlertMessage('차종 정보를 불러오지 못했습니다.')
        setShowAlert(true)
        setLoading(false)
      })
  }, [manufacturer])

  const onLayout = useCallback((e: LayoutChangeEvent) => {
    const {layout} = e.nativeEvent
    setTextMarginLeft(layout.x)
    setListCheck(true)
  }, [])

  const children = carTypeList.map((cartype, index) => {
    if (index == 0) {
      return (
        <TouchableView
          viewStyle={[styles.touchableView]}
          onPress={() => carType_onClick(cartype)}
          key={index.toString()}>
          <Text style={[styles.text]} onLayout={onLayout}>
            {cartype}
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
          onPress={() => carType_onClick(cartype)}
          key={index.toString()}>
          <Text style={[styles.text]}>{cartype}</Text>
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
                현재 자신의{'\n'}차종을 선택해주세요.
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
