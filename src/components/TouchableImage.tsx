import React from 'react'
import type {FC, ComponentProps} from 'react'
import {TouchableOpacity} from 'react-native'
import FastImage from 'react-native-fast-image'

type ImageProps = ComponentProps<typeof FastImage>

export type TouchableImageProps = ImageProps & {
  onPress: () => void
}

export const TouchableImage: FC<TouchableImageProps> = ({
  onPress,
  ...imageProps
}) => {
  return (
    <TouchableOpacity onPress={onPress}>
      <FastImage {...imageProps} />
    </TouchableOpacity>
  )
}
