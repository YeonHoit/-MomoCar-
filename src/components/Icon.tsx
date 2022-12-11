import React from 'react'
import type {FC, ComponentProps} from 'react'
import {Pressable} from 'react-native'
import FastImage from 'react-native-fast-image'

type ImageProps = ComponentProps<typeof FastImage>

// 커스텀 아이콘 컴포넌트 매개변수 정의
export type IconProps = ImageProps & {
  size: number // 아이콘 가로, 세로 길이
  onPress?: () => void // 아이콘 클릭 이벤트 함수
}

// 커스텀 아이콘 컴포넌트 정의
export const Icon: FC<IconProps> = ({size, onPress, ...imageProps}) => {
  return (
    <Pressable onPress={onPress}>
      <FastImage
        style={[{width: size, height: size}]}
        resizeMode="contain"
        {...imageProps}
      />
    </Pressable>
  )
}
