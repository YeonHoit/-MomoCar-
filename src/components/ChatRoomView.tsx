import React, {useEffect, useState, useMemo} from 'react'
import type {FC} from 'react'
import {StyleSheet, Text, View, Dimensions} from 'react-native'
import {TouchableView} from './TouchableView'
import type {TouchableViewProps} from './TouchableView'
import type {ChatRoom} from '../type'
import {momentCal} from '../utils'
import {Icon} from '../components/Icon'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post, serverUrl} from '../server'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  touchableView: {
    width: Dimensions.get('window').width,
    height: 70,
    backgroundColor: 'white',
    paddingHorizontal: 10,
    paddingVertical: 10,
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderColor: '#EEEEEE'
  },
  profileImage: {
    width: 50,
    height: 50,
    borderRadius: 35
  },
  newChatView: {
    width: 20,
    height: 20,
    borderRadius: 10,
    backgroundColor: '#FF0000',
    position: 'absolute',
    alignItems: 'center',
    justifyContent: 'center'
  },
  contentView: {
    flex: 1,
    alignItems: 'center',
    flexDirection: 'row'
  },
  opponent: {
    fontSize: 16,
    flex: 1
  },
  lastTime: {
    fontSize: 11,
    color: '#767676',
    marginRight: 10
  },
  lastChat: {
    fontSize: 14,
    color: '#767676'
  }
})

// 채팅방 컴포넌트 매개변수 정의
export type ChatRoomViewProps = TouchableViewProps & {
  chatRoomInfo: ChatRoom // 채팅방 정보 매개변수
  iconPress: () => void // 차단, 신고등의 아이콘 클릭 이벤트 함수
}

// 채팅방 컴포넌트 정의
export const ChatRoomView: FC<ChatRoomViewProps> = ({
  chatRoomInfo,
  iconPress,
  ...touchableProps
}) => {
  const [opponentNickName, setOpponentNickName] = useState<string>('') // 채팅 상대 이름 문자열
  const [userPhoto, setUserPhoto] = useState<string>('') // 채팅 상대의 프로필 이미지 문자열

  const user = useSelector<AppState, Ur.State>((state) => state.user)

  // 채팅 상대의 프로필 이미지 경로를 반환하는 useMemo
  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  // 채팅 상대 이름을 가져오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('ID', chatRoomInfo.Opponent)
    post('GetUserNickName.php', data)
      .then((UserNickNameInfo) => UserNickNameInfo.json())
      .then((UserNickNameInfoObject) => {
        const {GetUserNickName} = UserNickNameInfoObject
        setOpponentNickName(GetUserNickName[0].Nickname)
      })
      .catch((e) => {})
  }, [chatRoomInfo])

  // 채팅 상대의 사진 경로 문자열을 가져오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('NickName', opponentNickName)
    post('GetCommunityUserPhoto.php', data)
      .then((CommunityUserPhotoInfo) => CommunityUserPhotoInfo.json())
      .then((CommunityUserPhotoInfoObject) => {
        const {GetCommunityUserPhoto} = CommunityUserPhotoInfoObject
        setUserPhoto(GetCommunityUserPhoto[0].Photo)
      })
  }, [opponentNickName])

  return (
    <TouchableView viewStyle={[styles.touchableView]} {...touchableProps}>
      {/* 채팅 상대 프로필 이미지 및 새로 도착한 메세지 여부 표시 */}
      <View style={[{width: 50, height: 50}]}>
        {/* 채팅 상대 프로필 이미지 */}
        <FastImage source={profileImage} style={[styles.profileImage]} />
        {/* 새로 도착한 메세지 여부 표시 */}
        {chatRoomInfo.LastChat.ReadCheck == 0 &&
          chatRoomInfo.LastChat.ReceiveUserID == user.ID && (
            <View style={[styles.newChatView]}>
              <Text
                style={[{fontSize: 12, color: 'white', fontWeight: 'bold'}]}>
                N
              </Text>
            </View>
          )}
      </View>
      <View style={{flex: 1, marginLeft: 15}}>
        <View style={[styles.contentView]}>
          {/* 채팅 상대의 닉네임 */}
          <Text style={[styles.opponent]}>{opponentNickName}</Text>
          {/* 채팅 상대와의 마지막 채팅 시간 */}
          <Text style={[styles.lastTime]}>
            {momentCal(chatRoomInfo.LastChat.SendDate)}
          </Text>
          {/* 신고, 차단등을 지원하는 아이콘 */}
          <Icon
            source={require('../assets/images/three_dots_vertical.png')}
            size={25}
            onPress={iconPress}
          />
        </View>
        <View style={[styles.contentView]}>
          {/* 채팅 상대와의 마지막 채팅 내용 */}
          <Text
            style={[styles.lastChat]}
            numberOfLines={1}
            ellipsizeMode="tail">
            {chatRoomInfo.LastChat.Message}
          </Text>
        </View>
      </View>
    </TouchableView>
  )
}
