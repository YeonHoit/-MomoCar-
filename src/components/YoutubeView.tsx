import React from 'react'
import type {FC} from 'react'
import {StyleSheet, Text, Dimensions, View} from 'react-native'
import {TouchableView} from './TouchableView'
import type {TouchableViewProps} from './TouchableView'
import type {YoutubeType} from '../type'
import Color from 'color'
import {serverUrl} from '../server'
import AutoHeightImage from 'react-native-auto-height-image'

const styles = StyleSheet.create({
  view: {
    width: Dimensions.get('window').width - 20,
    marginLeft: 10,
    marginRight: 10
  },
  touchableView: {
    backgroundColor: 'white',
    width: Dimensions.get('window').width - 20,
    marginTop: 10,
    borderRadius: 20,
    padding: 10,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: Color('#DBDBDB').lighten(0.1).toString()
  },
  image: {
    width: Dimensions.get('window').width - 50
  },
  titleText: {
    fontSize: 14,
    marginBottom: 5
  },
  channelText: {
    fontSize: 14,
    color: '#767676'
  }
})

// 유튜브 컴포넌트 매개변수 정의
export type YoutubeViewProps = TouchableViewProps & {
  youtubeInfo: YoutubeType // 유튜브 정보 매개변수
}

// 유튜브 컴포넌트 정의
export const YoutubeView: FC<YoutubeViewProps> = ({
  youtubeInfo,
  ...touchableProps
}) => {
  return (
    <View style={[styles.view]}>
      <TouchableView viewStyle={[styles.touchableView]} {...touchableProps}>
        {/* 유튜브 썸네일 이미지 */}
        <AutoHeightImage
          width={Dimensions.get('window').width - 40}
          source={{uri: serverUrl + 'img/' + youtubeInfo.Photo}}
          resizeMode="contain"
          style={{borderRadius: 4}}
        />
        <View
          style={{
            width: Dimensions.get('window').width - 50,
            marginTop: 10,
            marginLeft: 10
          }}>
          {/* 유튜브 영상 제목 */}
          <Text
            style={[styles.titleText]}
            ellipsizeMode="tail"
            numberOfLines={1}>
            {youtubeInfo.Title}
          </Text>
          {/* 유튜브 채널 이름 */}
          <Text style={[styles.channelText]}>{youtubeInfo.Channel}</Text>
        </View>
      </TouchableView>
    </View>
  )
}
