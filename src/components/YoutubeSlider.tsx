import React, {useCallback, useState} from 'react'
import type {FC} from 'react'
import {StyleSheet, View} from 'react-native'
import type {LayoutChangeEvent} from 'react-native'
import {YoutubeView} from './YoutubeView'
import type {YoutubeType} from '../type'
import {useNavigation} from '@react-navigation/native'
import Swiper from 'react-native-swiper'

const styles = StyleSheet.create({
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
  }
})

// 유튜브 슬라이더 컴포넌트 매개변수 정의
export type YoutubeSliderProps = {
  youtubeViewInfo: YoutubeType[] // 유튜브 정보 리스트 매개변수
  youtubeWidth: number // Swiper 넓이 매개변수
}

// 유튜브 슬라이더 컴포넌트 정의
export const YoutubeSlider: FC<YoutubeSliderProps> = ({
  youtubeViewInfo,
  youtubeWidth
}) => {
  const navigation = useNavigation()
  const [swiperHeight, setSwiperHeight] = useState<number>(0)

  const onLayout = useCallback((e: LayoutChangeEvent) => {
    const {layout} = e.nativeEvent
    setSwiperHeight(+layout.height + 30)
  }, [])

  return (
    <Swiper
      horizontal
      width={youtubeWidth}
      height={swiperHeight}
      autoplay={false}
      dot={
        <View
          style={[
            styles.swiperdot,
            {width: (youtubeWidth - 150) / youtubeViewInfo.length}
          ]}
        />
      }
      activeDot={
        <View
          style={[
            styles.activedot,
            {width: (youtubeWidth - 150) / youtubeViewInfo.length}
          ]}
        />
      }>
      {youtubeViewInfo.map((item, index) => {
        if (index == 0) {
          return (
            <YoutubeView
              youtubeInfo={item}
              onPress={() =>
                navigation.navigate('YoutubeWebView', {youtubeInfo: item})
              }
              key={index.toString()}
              onLayout={onLayout}
            />
          )
        } else {
          return (
            <YoutubeView
              youtubeInfo={item}
              onPress={() =>
                navigation.navigate('YoutubeWebView', {youtubeInfo: item})
              }
              key={index.toString()}
            />
          )
        }
      })}
    </Swiper>
  )
}
