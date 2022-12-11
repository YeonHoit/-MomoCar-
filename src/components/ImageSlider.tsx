import React, {useRef, useMemo, useCallback} from 'react'
import type {FC} from 'react'
import {StyleSheet, FlatList, View, Animated, Dimensions} from 'react-native'
import type {NativeSyntheticEvent, NativeScrollEvent} from 'react-native'
import {useAnimatedValue, useTransformStyle} from '../hooks'
import FastImage from 'react-native-fast-image'

let circleMarginRight = 5

const styles = StyleSheet.create({
  image: {
    height: 200,
    resizeMode: 'cover'
  },
  iconBar: {
    flexDirection: 'row',
    paddingVertical: 5
  },
  circle: {
    height: 5,
    borderRadius: 5,
    marginRight: circleMarginRight,
    backgroundColor: '#F6F6F6'
  },
  selectedCircle: {
    position: 'absolute',
    backgroundColor: '#DD0D48'
  }
})

export type ImageSliderProps = {
  imageUrls: string[]
  imageWidth: number
}

export const ImageSlider: FC<ImageSliderProps> = ({imageUrls, imageWidth}) => {
  const flatListRef = useRef<FlatList | null>(null)
  const selectedIndexAnimValue = useAnimatedValue(0)
  const circleWidth =
    Math.trunc(Dimensions.get('window').width / imageUrls.length) - 5

  const circleWidthAnimValue = useAnimatedValue(circleWidth)
  const circleMarginRightAnimValue = useAnimatedValue(circleMarginRight)

  const onScroll = useCallback(
    (event: NativeSyntheticEvent<NativeScrollEvent>) => {
      if (imageWidth == 0) {
        return
      }
      const {contentOffset} = event.nativeEvent
      const index = Math.round(contentOffset.x / imageWidth)
      selectedIndexAnimValue.setValue(index)
    },
    [imageWidth]
  )

  const circles = useMemo(
    () =>
      imageUrls.map((uri, index) => (
        <View key={index} style={[styles.circle, {width: circleWidth}]} />
      )),
    [circleWidth]
  )

  const translateX = useTransformStyle(
    {
      translateX: Animated.multiply(
        selectedIndexAnimValue,
        Animated.add(circleWidthAnimValue, circleMarginRightAnimValue)
      )
    },
    [circleWidthAnimValue]
  )

  return (
    <>
      <FlatList
        ref={flatListRef}
        scrollEnabled={true}
        pagingEnabled={true}
        onScroll={onScroll}
        contentContainerStyle={[{width: imageUrls.length * imageWidth}]}
        showsHorizontalScrollIndicator={false}
        horizontal={true}
        data={imageUrls}
        renderItem={({item}) => (
          <FastImage
            style={[styles.image, {width: imageWidth}]}
            source={{uri: item}}
            resizeMode="contain"
          />
        )}
        keyExtractor={(item, index) => index.toString()}
      />
      <View style={[styles.iconBar, {justifyContent: 'center'}]}>
        <View style={[{flexDirection: 'row'}]}>
          {circles}
          <Animated.View
            style={[
              styles.circle,
              styles.selectedCircle,
              translateX,
              {width: circleWidth}
            ]}
          />
        </View>
      </View>
    </>
  )
}
