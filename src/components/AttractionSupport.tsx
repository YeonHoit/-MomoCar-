import React from 'react'
import type {FC} from 'react'
import {Dimensions, StyleSheet, View} from 'react-native'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  imageView: {
    padding: 10,
    width: (Dimensions.get('window').width - 40) / 4,
    height: (Dimensions.get('window').width - 40) / 4
  }
})

// 명소의 편의 시설 매개변수 정의
export type AttractionSupportProps = {
  name: string // 편의 시설 이름 매개변수
}

// 명소의 편의 시설 정의
export const AttractionSupport: FC<AttractionSupportProps> = ({name}) => {
  if (name === '주차장') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/parkinglot.png')}
        />
      </View>
    )
  }
  if (name === '와이파이') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/wifi.png')}
        />
      </View>
    )
  }
  if (name === '테라스') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/terrace.png')}
        />
      </View>
    )
  }
  if (name === '루프탑') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/looftop.png')}
        />
      </View>
    )
  }
  if (name === '대형카페') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/bigcafe.png')}
        />
      </View>
    )
  }
  if (name === '반려동물 동반') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/canpet.png')}
        />
      </View>
    )
  }
  if (name === '단체석') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/partyroom.png')}
        />
      </View>
    )
  }
  if (name === '포장') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/packaging.png')}
        />
      </View>
    )
  }
  if (name === '예약') {
    return (
      <View style={[styles.imageView]}>
        <FastImage
          style={{flex: 1}}
          source={require('../assets/images/reservation.png')}
        />
      </View>
    )
  }
  return null
}
