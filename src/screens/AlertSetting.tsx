import React, {useCallback, useState} from 'react'
import {StyleSheet, Text, View, Dimensions, Switch} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {NavigationHeader, Icon, AlertComponent} from '../components'
import {useNavigation} from '@react-navigation/native'
import {useToggle} from '../hooks'
import {useSelector, useDispatch} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post} from '../server'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  bigText: {
    marginLeft: 20,
    marginTop: 10,
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 50
  },
  settingView: {
    width: Dimensions.get('window').width - 40,
    height: 50,
    marginLeft: 20,
    marginTop: 10,
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 10
  },
  switchText: {
    fontSize: 15,
    fontWeight: '500',
    flex: 1
  }
})

// 알림 설정 화면
export default function AlertSetting() {
  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const dispatch = useDispatch()

  const [notice, toggleNotice] = useToggle(user.NoticeAlert) // 공지사항 알림 여부
  const [comment, toggleComment] = useToggle(user.CommentAlert) // 댓글 알림 여부
  const [like, toggleLike] = useToggle(user.LikeAlert) // 좋아요 알림 여부
  const [tag, toggleTag] = useToggle(user.TagAlert) // 태그 알림 여부
  const [chatting, toggleChatting] = useToggle(user.ChattingAlert) // 채팅 알림 여부
  const [alertMessage, setAlertMessage] = useState<string>('') // 알림창 메세지 문자열
  const [showAlert, setShowAlert] = useState<boolean>(false) // 알림창 표시 여부

  // 뒤로 가기 버튼 클릭 이벤트 함수
  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  // 공지사항 알림 클릭 이벤트 함수
  const noticeUpdate = useCallback(() => {
    let temp = notice
    toggleNotice()
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('target', 'NoticeAlert')
    data.append('value', !temp)
    data.append('before', user.Nickname)
    post('UpdateUserInfo.php', data)
      .then(() => {
        dispatch(Ur.setUserAction({...user, NoticeAlert: !temp}))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
        setShowAlert(true)
      })
  }, [user, notice])

  // 댓글 알림 클릭 이벤트 함수
  const commentUpdate = useCallback(() => {
    let temp = comment
    toggleComment()
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('target', 'CommentAlert')
    data.append('value', !temp)
    data.append('before', user.Nickname)
    post('UpdateUserInfo.php', data)
      .then(() => {
        dispatch(Ur.setUserAction({...user, CommentAlert: !temp}))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
        setShowAlert(true)
      })
  }, [user, comment])

  // 좋아요 알림 클릭 이벤트 함수
  const likeUpdate = useCallback(() => {
    let temp = like
    toggleLike()
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('target', 'LikeAlert')
    data.append('value', !temp)
    data.append('before', user.Nickname)
    post('UpdateUserInfo.php', data)
      .then(() => {
        dispatch(Ur.setUserAction({...user, LikeAlert: !temp}))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
        setShowAlert(true)
      })
  }, [user, like])

  // 태그 알림 클릭 이벤트 함수
  const tagUpdate = useCallback(() => {
    let temp = tag
    toggleTag()
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('target', 'TagAlert')
    data.append('value', !temp)
    data.append('before', user.Nickname)
    post('UpdateUserInfo.php', data)
      .then(() => {
        dispatch(Ur.setUserAction({...user, TagAlert: !temp}))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
        setShowAlert(true)
      })
  }, [user, tag])

  // 채팅 알림 클릭 이벤트 함수
  const chattingUpdate = useCallback(() => {
    let temp = chatting
    toggleChatting()
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('target', 'ChattingAlert')
    data.append('value', !temp)
    data.append('before', user.Nickname)
    post('UpdateUserInfo.php', data)
      .then(() => {
        dispatch(Ur.setUserAction({...user, ChattingAlert: !temp}))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
        setShowAlert(true)
      })
  }, [user, tag])

  return (
    <>
      <SafeAreaView style={[styles.view]}>
        {/* 화면 상단 바 */}
        <NavigationHeader
          viewStyle={[{backgroundColor: 'white'}]}
          Left={() => (
            <Icon
              source={require('../assets/images/arrow_left.png')}
              size={30}
              onPress={goBack}
            />
          )}
        />
        <Text style={[styles.bigText]}>알림설정</Text>
        <View style={[styles.settingView]}>
          <Text style={[styles.switchText]}>공지 알림</Text>
          {/* 공지 사항 알림 스위치 */}
          <Switch
            value={notice}
            onValueChange={noticeUpdate}
            trackColor={{false: '#D9D9DF', true: '#E6135A'}}
            thumbColor="white"
          />
        </View>
        <View style={[styles.settingView]}>
          <Text style={[styles.switchText]}>댓글 알림</Text>
          {/* 댓글 알림 스위치 */}
          <Switch
            value={comment}
            onValueChange={commentUpdate}
            trackColor={{false: '#D9D9DF', true: '#E6135A'}}
            thumbColor="white"
          />
        </View>
        <View style={[styles.settingView]}>
          <Text style={[styles.switchText]}>좋아요 알림</Text>
          {/* 좋아요 알림 스위치 */}
          <Switch
            value={like}
            onValueChange={likeUpdate}
            trackColor={{false: '#D9D9DF', true: '#E6135A'}}
            thumbColor="white"
          />
        </View>
        <View style={[styles.settingView]}>
          <Text style={[styles.switchText]}>댓글 태그 알림</Text>
          {/* 태그 알림 스위치 */}
          <Switch
            value={tag}
            onValueChange={tagUpdate}
            trackColor={{false: '#D9D9DF', true: '#E6135A'}}
            thumbColor="white"
          />
        </View>
        <View style={[styles.settingView]}>
          <Text style={[styles.switchText]}>채팅 알림</Text>
          {/* 채팅 알림 스위치 */}
          <Switch
            value={chatting}
            onValueChange={chattingUpdate}
            trackColor={{false: '#D9D9DF', true: '#E6135A'}}
            thumbColor="white"
          />
        </View>
      </SafeAreaView>
      {/* 알림 창 */}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
    </>
  )
}
