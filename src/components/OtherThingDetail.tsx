import React from 'react'
import type {FC} from 'react'
import {StyleSheet, Text, View, Dimensions} from 'react-native'
import {TouchableView} from '../components'
import type {TouchableViewProps} from './TouchableView'
import type {CommunityPost} from '../type'
import {serverUrl} from '../server'
import {priceComma} from '../utils'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view_left: {
    width: Dimensions.get('window').width / 3,
    paddingLeft: (Dimensions.get('window').width / 28 / 3) * 4,
    marginBottom: 10,
    marginTop: 5
  },
  view_center: {
    width: Dimensions.get('window').width / 3,
    paddingLeft: (Dimensions.get('window').width / 28 / 3) * 2,
    marginBottom: 10,
    marginTop: 5
  },
  view_right: {
    width: Dimensions.get('window').width / 3,
    marginBottom: 10,
    marginTop: 5
  },
  image: {
    width: (Dimensions.get('window').width / 28) * 8,
    height: 150,
    borderRadius: 5
  },
  title: {
    fontSize: 12,
    color: '#767676'
  },
  price: {
    fontSize: 14,
    fontWeight: 'bold'
  },
  textView: {
    width: (Dimensions.get('window').width / 28) * 8,
    marginTop: 10
  }
})

// 다른 팜매 품목 자세히 보기 컴포넌트 매개변수 정의
export type OtherThingDetailProps = TouchableViewProps & {
  communityPostInfo: CommunityPost // 다른 판매 품목 글 정보 매개변수
  indexNumber: number // 다른 판매 품목 배열의 현재 번호 매개변수
}

// 다른 판매 품목 자세히 보기 컴포넌트 정의
export const OtherThingDetail: FC<OtherThingDetailProps> = ({
  communityPostInfo,
  indexNumber,
  ...touchableProps
}) => {
  let photoList: string[] = communityPostInfo.Photo.split('|') // 다른 판매 품목 글의 첨부 사진 배열

  if (indexNumber % 3 == 0) {
    // 왼쪽 렌더링일 때
    return (
      <TouchableView viewStyle={[styles.view_left]} {...touchableProps}>
        {/* 다른 판매 품목 첫번째 사진 */}
        {communityPostInfo.Photo.length !== 0 && (
          <FastImage
            source={{uri: serverUrl + 'img/' + photoList[0]}}
            style={[styles.image]}
            resizeMode="cover"
          />
        )}
        <View style={[styles.textView]}>
          {/* 가격 */}
          <Text style={[styles.price]}>
            {priceComma(communityPostInfo.Price)}원
          </Text>
          {/* 다른 판매 품목의 제목 */}
          <Text style={[styles.title]} numberOfLines={1}>
            {communityPostInfo.Title}
          </Text>
        </View>
      </TouchableView>
    )
  }

  if (indexNumber % 3 == 1) {
    // 가운데 렌더링일 때
    return (
      <TouchableView viewStyle={[styles.view_center]} {...touchableProps}>
        {/* 다른 판매 품목 첫번째 사진 */}
        {communityPostInfo.Photo.length !== 0 && (
          <FastImage
            source={{uri: serverUrl + 'img/' + photoList[0]}}
            style={[styles.image]}
            resizeMode="cover"
          />
        )}
        <View style={[styles.textView]}>
          {/* 가격 */}
          <Text style={[styles.price]}>
            {priceComma(communityPostInfo.Price)}원
          </Text>
          {/* 다른 판매 품목의 제목 */}
          <Text style={[styles.title]} numberOfLines={1}>
            {communityPostInfo.Title}
          </Text>
        </View>
      </TouchableView>
    )
  }

  // 오른쪽 렌더링일 때
  return (
    <TouchableView viewStyle={[styles.view_right]} {...touchableProps}>
      {/* 다른 판매 품목 첫번째 사진 */}
      {communityPostInfo.Photo.length !== 0 && (
        <FastImage
          source={{uri: serverUrl + 'img/' + photoList[0]}}
          style={[styles.image]}
          resizeMode="cover"
        />
      )}
      <View style={[styles.textView]}>
        {/* 가격 */}
        <Text style={[styles.price]}>
          {priceComma(communityPostInfo.Price)}원
        </Text>
        {/* 다른 판매 품목의 제목 */}
        <Text style={[styles.title]} numberOfLines={1}>
          {communityPostInfo.Title}
        </Text>
      </View>
    </TouchableView>
  )
}
