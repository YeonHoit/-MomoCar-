import React, {useEffect, useState, useMemo, useCallback} from 'react'
import {
  StyleSheet,
  FlatList,
  View,
  Modal,
  Pressable,
  Dimensions,
  Text
} from 'react-native'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post} from '../server'
import type {Chat, ChatRoom} from '../type'
import {arrayCopyAndPush, chatSort} from '../utils'
import {
  ChatRoomView,
  AlertComponent,
  TouchableView,
  Loading
} from '../components'
import {Colors} from 'react-native-paper'
import color from 'color'
import {useNavigation} from '@react-navigation/native'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: '#F8F8FA'
  },
  itemSeparator: {
    borderWidth: 1,
    borderColor: color(Colors.grey500).lighten(0.3).string()
  },
  absoluteView: {
    flex: 1,
    backgroundColor: color('#000000').alpha(0.45).string()
  },
  selectBar: {
    width: '100%',
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20
  },
  selectItem: {
    width: Dimensions.get('window').width - 40,
    height: 70,
    marginLeft: 20,
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    alignItems: 'center'
  },
  selectItemText: {
    marginLeft: 10,
    fontSize: 14
  }
})

export default function Chatting() {
  const [chattingList, setChattingList] = useState<Chat[]>([])
  const [chatCount, setChatCount] = useState<number>(0)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [showProfileBar, setShowProfileBar] = useState<boolean>(false)
  const [showBlockAlert, setShowBlockAlert] = useState<boolean>(false)
  const [blockTarget, setBlockTarget] = useState<string>('')
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false)
  const [chattingListLoading, setChattingListLoading] = useState<boolean>(true)
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {ID} = user
  const navigation = useNavigation()

  //채팅 내역을 분리하여 저장한 Map Object
  const ChatMap = useMemo(() => {
    const map = new Map<string, Chat[]>()
    for (let chat of chattingList) {
      //내가 보낸 채팅일 때
      if (chat.SendUserID === ID) {
        map.set(
          chat.ReceiveUserID,
          arrayCopyAndPush(map.get(chat.ReceiveUserNickName), chat)
        )
      }
      //상대방이 보낸 채팅일 때
      else if (chat.ReceiveUserID === ID) {
        map.set(
          chat.SendUserID,
          arrayCopyAndPush(map.get(chat.SendUserNickName), chat)
        )
      }
    }
    return map
  }, [chattingList, user])

  const ChatRoomList = useMemo(() => {
    let list: ChatRoom[] = []
    ChatMap.forEach((value, key, mapObject) => {
      list.push({Opponent: key, LastChat: value[value.length - 1]})
    })
    list.sort(chatSort)
    return list
  }, [ChatMap])

  const ChatRoomOnClick = useCallback((item: ChatRoom) => {
    navigation.navigate('ChatRoomScreen', {chatRoomInfo: item})
  }, [])

  const blockUser = useCallback(() => {
    setShowBlockAlert(false)
    let data = new FormData()
    data.append('UserID', user.ID)
    data.append('Target', blockTarget)
    post('InsertBlock.php', data)
      .then(() => {
        setAlertMessage('차단이 완료되었습니다.')
        setShowAlert(true)
      })
      .catch((e) => {
        setAlertMessage('사용자 차단에 실패하였습니다.')
        setShowAlert(true)
      })
  }, [blockTarget, user])

  const reportPress = useCallback(() => {
    setShowProfileBar(false)
    setShowReportAlert(true)
  }, [])

  useEffect(() => {
    const interval_id = setInterval(() => {
      let data = new FormData()
      data.append('ID', ID)
      post('GetChattingCount.php', data)
        .then((ChattingCountInfo) => ChattingCountInfo.json())
        .then((ChattingCountInfoObject) => {
          const {GetChattingCount} = ChattingCountInfoObject
          if (chatCount != GetChattingCount.Count) {
            let data = new FormData()
            data.append('ID', ID)
            post('GetChattingList.php', data)
              .then((ChattingListInfo) => ChattingListInfo.json())
              .then((ChattingListInfoObject) => {
                const {GetChattingList} = ChattingListInfoObject
                setChattingList(GetChattingList)
              })
              .then(() => setChattingListLoading(false))
              .catch((e) => {
                setAlertMessage('채팅 리스트를 불러오지 못했습니다.')
                setShowAlert(true)
                setChattingListLoading(false)
              })
            setChatCount(GetChattingCount.Count)
          }
        })
        .catch(() => {})
    }, 1000)
    return () => clearInterval(interval_id)
  }, [user])

  return (
    <>
      {chattingListLoading && <Loading />}
      {!chattingListLoading && (
        <View style={[styles.view]}>
          <FlatList
            data={ChatRoomList}
            renderItem={({item}) => (
              <ChatRoomView
                chatRoomInfo={item}
                onPress={() => ChatRoomOnClick(item)}
                iconPress={() => {
                  setBlockTarget(item.Opponent)
                  setShowProfileBar(true)
                }}
              />
            )}
            keyExtractor={(item, index) => index.toString()}
          />
        </View>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
      {showProfileBar && (
        <Modal transparent={true}>
          <View style={[styles.absoluteView]}>
            <Pressable
              style={{flex: 1}}
              onPress={() => setShowProfileBar(false)}
            />
            <View style={[styles.selectBar]}>
              <TouchableView
                viewStyle={[styles.selectItem]}
                onPress={() => {
                  setShowProfileBar(false)
                  setShowBlockAlert(true)
                }}>
                <FastImage
                  style={{width: 35, height: 35}}
                  source={require('../assets/images/ban_background.png')}
                />
                <Text style={[styles.selectItemText]}>차단하기</Text>
              </TouchableView>
              <TouchableView
                viewStyle={[styles.selectItem]}
                onPress={reportPress}>
                <FastImage
                  style={[{width: 35, height: 35}]}
                  source={require('../assets/images/report_background.png')}
                />
                <Text style={[styles.selectItemText]}>신고하기</Text>
              </TouchableView>
            </View>
          </View>
        </Modal>
      )}
      {showBlockAlert && (
        <AlertComponent
          message="정말 차단하시겠습니까?"
          cancelUse={true}
          okPress={blockUser}
          cancelPress={() => setShowBlockAlert(false)}
        />
      )}
      {showReportAlert && (
        <AlertComponent
          message="정말 신고하시겠습니까?"
          cancelUse={true}
          okPress={() => {
            setShowReportAlert(false)
            setAlertMessage('신고가 접수 되었습니다.')
            setShowAlert(true)
          }}
          cancelPress={() => setShowReportAlert(false)}
        />
      )}
    </>
  )
}
