import React from 'react'
import type {FC, ComponentProps} from 'react'
import {Text, StyleSheet} from 'react-native'
import {TouchableView} from './TouchableView'
import type {TouchableViewProps} from './TouchableView'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  touchableView: {
    alignItems: 'center',
    justifyContent: 'center',
    width: 100,
    height: 100
  },
  view: {
    borderWidth: 1,
    borderColor: '#D3D3D3',
    borderRadius: 10,
    width: 70,
    height: 70
  },
  text: {
    fontSize: 12,
    textAlign: 'center',
    textAlignVertical: 'center',
    marginTop: 5,
    color: 'black'
  }
})

// 제조사 이미지 컴포넌트 매개변수 정의
export type ManufacturerImageProps = TouchableViewProps &
  ComponentProps<typeof FastImage> & {
    text: number | string // 제조사 이름 매개변수
  }

export const ManufacturerImage: FC<ManufacturerImageProps> = ({
  source,
  text,
  ...touchableProps
}) => {
  return (
    <TouchableView style={[styles.touchableView]} {...touchableProps}>
      <FastImage source={source} style={[styles.view]} />
      <Text style={[styles.text]}>{text}</Text>
    </TouchableView>
  )
}
