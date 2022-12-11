import React, {useEffect, useState, useCallback} from 'react'
import {StyleSheet, ScrollView, Text, View} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {post_without_data} from '../server'
import {
  ManufacturerImage,
  NavigationHeader,
  Icon,
  AlertComponent,
  Loading
} from '../components'
import {selectManufacturerImage} from '../utils'
import {useNavigation} from '@react-navigation/native'
import type {LayoutChangeEvent} from 'react-native'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  text: {
    fontSize: 22,
    color: '#191919',
    fontWeight: '800',
    marginVertical: 50
  },
  itemView: {
    flexWrap: 'wrap',
    flexDirection: 'row',
    justifyContent: 'center'
  }
})

export default function ChangeManufacturer() {
  //prettier-ignore
  const [manufacturerListLoading, setManufacturerListLoading] = useState<boolean>(true)
  const [manufacturerList, setManufacturerList] = useState<string[]>([])
  const [textMarginLeft, setTextMarginLeft] = useState<number>(0)
  const [listCheck, setListCheck] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)

  const navigation = useNavigation()

  const manufactuer_onclick = useCallback((manufacturer_select: string) => {
    navigation.navigate('SetCarType', {manufacturer: manufacturer_select})
  }, [])

  const onLayout = useCallback((e: LayoutChangeEvent) => {
    const {layout} = e.nativeEvent
    setTextMarginLeft(layout.x)
    setListCheck(true)
  }, [])

  const children = manufacturerList.map((manufacturer, index) => {
    if (index == 0) {
      return (
        <ManufacturerImage
          source={selectManufacturerImage(manufacturer)}
          text={manufacturer}
          onPress={() => manufactuer_onclick(manufacturer)}
          key={index.toString()}
          onLayout={onLayout}
        />
      )
    } else {
      return (
        <ManufacturerImage
          source={selectManufacturerImage(manufacturer)}
          text={manufacturer}
          onPress={() => manufactuer_onclick(manufacturer)}
          key={index.toString()}
        />
      )
    }
  })

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  useEffect(() => {
    post_without_data('GetManufacturer.php')
      .then((ManufacturerInfo) => ManufacturerInfo.json())
      .then((ManufacturerInfoObject) => {
        const {GetManufacturer} = ManufacturerInfoObject
        setManufacturerList(GetManufacturer)
      })
      .then(() => setManufacturerListLoading(false))
      .catch((e) => {
        setAlertMessage('제조사 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setManufacturerListLoading(false)
      })
  }, [])

  return (
    <>
      {manufacturerListLoading && <Loading />}
      {!manufacturerListLoading && (
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
              <Text style={[styles.text, {marginLeft: textMarginLeft + 15}]}>
                관심 차종을{'\n'}선택합니다.{'\n'}브랜드를 선택해주세요.
              </Text>
            )}
            <View style={[styles.itemView]}>{children}</View>
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
