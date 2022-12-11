import React, {useMemo} from 'react'
import type {FC, ComponentProps} from 'react'
import {StyleSheet, Text, View, Dimensions} from 'react-native'
import type {Chat} from '../type'
import {momentCal} from '../utils'
import AutoHeightImage from 'react-native-auto-height-image'
import {serverUrl} from '../server'
import {TouchableImage} from '../components'

const styles = StyleSheet.create({
  view: {
    width: Dimensions.get('window').width - 40,
    justifyContent: 'space-between',
    marginLeft: 20,
    paddingVertical: 10
  },
  emptyView: {
    width: 30
  },
  messageView: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 10,
    borderRadius: 10
  },
  message: {
    textAlign: 'center',
    fontSize: 20,
    padding: 10
  },
  datetext: {
    fontSize: 8
  },
  chatImage: {
    padding: 10
  }
})

type ViewProps = ComponentProps<typeof View>

// 채팅 메세지 컴포넌트 매개변수 정의
export type MessageViewProps = ViewProps & {
  userSend: boolean // 메세지가 사용자가 보낸 메세지인지에 대한 여부
  chatInfo: Chat // 메세지 정보 매개변수
  userPhoto: string // 채팅 상대의 프로필 이미지 매개변수
  profilePress: () => void // 프로필 이미지 클릭 이벤트 함수
}

// 채팅 메세지 컴포넌트 정의
export const MessageView: FC<MessageViewProps> = ({
  userSend,
  chatInfo,
  userPhoto,
  profilePress,
  ...viewProps
}) => {
  // 채팅 상대의 프로필 이미지 경로를 반환하는 useMemo
  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  //사용자가 보낸 채팅일 때
  if (userSend) {
    //사진일 때
    if (chatInfo.Photo.length > 0) {
      return (
        <View style={[styles.view, {flexDirection: 'row'}]} {...viewProps}>
          <View style={[styles.emptyView]} />
          <View style={[{flexDirection: 'row', alignItems: 'flex-end'}]}>
            <View style={[{alignItems: 'flex-end', marginRight: 10}]}>
              {/* 상대방이 읽은 여부 표시 */}
              {chatInfo.ReadCheck == 1 && (
                <Text style={[{fontSize: 10}]}>읽음</Text>
              )}
              {/* 채팅 시간 */}
              <Text style={[styles.datetext]}>
                {momentCal(chatInfo.SendDate)}
              </Text>
            </View>
            {/* 사진 */}
            <View style={[styles.messageView]}>
              <AutoHeightImage
                width={200}
                source={{uri: serverUrl + 'img/' + chatInfo.Photo}}
                resizeMode="contain"
              />
            </View>
          </View>
        </View>
      )
    }
    //메세지일 때
    else {
      return (
        <View style={[styles.view, {flexDirection: 'row'}]} {...viewProps}>
          <View style={[styles.emptyView]} />
          <View style={[{flexDirection: 'row', alignItems: 'flex-end'}]}>
            <View style={[{alignItems: 'flex-end', marginRight: 10}]}>
              {/* 상대방이 읽은 여부 표시 */}
              {chatInfo.ReadCheck == 1 && (
                <Text style={[{fontSize: 10}]}>읽음</Text>
              )}
              {/* 채팅 시간 */}
              <Text style={[styles.datetext]}>
                {momentCal(chatInfo.SendDate)}
              </Text>
            </View>
            {/* 채팅 내용 */}
            <View style={[styles.messageView, {backgroundColor: '#E6135A'}]}>
              <Text style={[{fontSize: 14, maxWidth: 250, color: 'white'}]}>
                {chatInfo.Message}
              </Text>
            </View>
          </View>
        </View>
      )
    }
  }
  //상대방이 보낸 채팅일 때
  //사진일 때
  if (chatInfo.Photo.length > 0) {
    return (
      <View style={[styles.view, {flexDirection: 'row'}]} {...viewProps}>
        <View style={{width: 60}}>
          {/* 채팅 상대의 프로필 사진 */}
          <TouchableImage
            source={profileImage}
            style={{width: 48, height: 48, borderRadius: 24}}
            onPress={profilePress}
          />
        </View>
        <View style={{flex: 1, marginLeft: 5}}>
          {/* 채팅 상대의 닉네임 */}
          <Text style={{fontSize: 14, marginTop: 5}}>
            {chatInfo.SendUserNickName}
          </Text>
          <View style={{flex: 1, flexDirection: 'row', alignItems: 'flex-end'}}>
            <View
              style={[
                styles.messageView,
                {backgroundColor: 'white', marginTop: 10}
              ]}>
              {/* 사진 */}
              <AutoHeightImage
                width={200}
                source={{uri: serverUrl + 'img/' + chatInfo.Photo}}
                style={[styles.chatImage]}
                resizeMode="contain"
              />
            </View>
            {/* 채팅 시간 */}
            <Text style={[styles.datetext, {marginLeft: 10}]}>
              {momentCal(chatInfo.SendDate)}
            </Text>
          </View>
        </View>
      </View>
    )
  }
  //메세지일 때
  return (
    <View style={[styles.view, {flexDirection: 'row'}]} {...viewProps}>
      <View style={{width: 60}}>
        {/* 채팅 상대의 프로필 사진 */}
        <TouchableImage
          source={profileImage}
          style={{width: 48, height: 48, borderRadius: 24}}
          onPress={profilePress}
        />
      </View>
      <View style={{flex: 1, marginLeft: 5}}>
        {/* 채팅 상대의 닉네임 */}
        <Text style={{fontSize: 14, marginTop: 5}}>
          {chatInfo.SendUserNickName}
        </Text>
        <View style={{flex: 1, flexDirection: 'row', alignItems: 'flex-end'}}>
          <View
            style={[
              styles.messageView,
              {backgroundColor: 'white', marginTop: 10}
            ]}>
            {/* 채팅 내용 */}
            <Text style={{fontSize: 14, maxWidth: 220}}>
              {chatInfo.Message}
            </Text>
          </View>
          {/* 채팅 시간 */}
          <Text style={[styles.datetext, {marginLeft: 10}]}>
            {momentCal(chatInfo.SendDate)}
          </Text>
        </View>
      </View>
    </View>
  )
}
