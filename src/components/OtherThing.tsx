import React from 'react'
import type {FC} from 'react'
import {StyleSheet, Text} from 'react-native'
import {TouchableView} from '../components'
import type {TouchableViewProps} from './TouchableView'
import type {CommunityPost} from '../type'
import {serverUrl} from '../server'
import {priceComma} from '../utils'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    width: 150
  },
  image: {
    width: 130,
    height: 130,
    borderRadius: 5
  },
  title: {
    width: 130,
    fontSize: 14,
    paddingVertical: 3,
    color: '#767676'
  },
  price: {
    fontSize: 15,
    fontWeight: 'bold'
  }
})

// 다른 판매 품목 컴포넌트 매개변수 정의
export type OtherThingProps = TouchableViewProps & {
  communityPostInfo: CommunityPost // 다른 판매 품목 글 정보 매개변수
}

// 다른 판매 품목 컴포넌트 정의
export const OtherThing: FC<OtherThingProps> = ({
  communityPostInfo,
  ...touchableProps
}) => {
  let photoList: string[] = communityPostInfo.Photo.split('|') // 다른 판매 품목 글의 첨부 사진 배열

  return (
    <TouchableView viewStyle={[styles.view]} {...touchableProps}>
      {/* 다른 판매 품목 첫번째 첨부 사진 */}
      {communityPostInfo.Photo.length !== 0 && (
        <FastImage
          source={{uri: serverUrl + 'img/' + photoList[0]}}
          style={[styles.image]}
        />
      )}
      {/* 다른 판매 품목 가격 */}
      <Text style={[styles.price]}>
        {priceComma(communityPostInfo.Price)}원
      </Text>
      {/* 다른 판매 품목 제목 */}
      <Text style={[styles.title]} numberOfLines={1}>
        {communityPostInfo.Title}
      </Text>
    </TouchableView>
  )
}
