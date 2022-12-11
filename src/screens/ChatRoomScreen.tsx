import React, {useCallback, useEffect, useState, useRef, useMemo} from 'react'
import {
  StyleSheet,
  View,
  TextInput,
  Platform,
  ScrollView,
  Dimensions,
  Pressable,
  Text
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  MessageView,
  Icon,
  AlertComponent,
  TouchableView,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {Chat} from '../type'
import {post} from '../server'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {AutoFocusProvider, useAutoFocus} from '../contexts'
import {Colors} from 'react-native-paper'
import color from 'color'
import {launchImageLibrary} from 'react-native-image-picker'
import type {ImageLibraryOptions} from 'react-native-image-picker'
import type {ParamList} from '../navigator/MainNavigator'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  flex: {
    flex: 1,
    backgroundColor: 'white'
  },
  contentView: {
    flex: 1,
    backgroundColor: '#F8F8FA'
  },
  text: {
    fontSize: 14
  },
  view: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center'
  },
  sendchatview: {
    width: '100%',
    borderTopWidth: 1,
    borderTopColor: color(Colors.grey500).lighten(0.3).string(),
    paddingVertical: 10,
    paddingRight: 20,
    paddingLeft: 10,
    flexDirection: 'row',
    backgroundColor: 'white'
  },
  textInput: {
    flex: 1,
    paddingHorizontal: 10,
    fontSize: 18,
    borderWidth: 1,
    borderColor: '#E6135A',
    textAlignVertical: 'top',
    borderRadius: 10,
    marginRight: 15,
    paddingBottom: 10,
    paddingTop: 10
  },
  absoluteView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
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

export default function ChatRoomScreen() {
  const [chattingList, setChattingList] = useState<Chat[]>([])
  const [photoUri, setPhotoUri] = useState<string>('')
  const [chat, setChat] = useState<string>('')
  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'ChatRoomScreen'>>()
  const chatRoomInfo = route.params.chatRoomInfo
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {ID, Nickname} = user
  const focus = useAutoFocus()
  const scrollViewRef = useRef<ScrollView | null>(null)
  const [height, setHeight] = useState<number>(18)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [opponentNickName, setOpponentNickName] = useState<string>('')
  const [userPhoto, setUserPhoto] = useState<string>('')
  const [showProfileBar, setShowProfileBar] = useState<boolean>(false)
  const [showBlockAlert, setShowBlockAlert] = useState<boolean>(false)
  const [showReportAlert, setShowReportAlert] = useState<boolean>(false)
  const [chattingListLoading, setChattingListLoading] = useState<boolean>(true)

  const loading = useMemo(() => {
    if (!chattingListLoading && opponentNickName.length != 0) {
      return false
    }
    return true
  }, [chattingListLoading, opponentNickName])

  const chatViewList = useMemo(() => {
    return chattingList.map((item, index) => (
      <MessageView
        userSend={Nickname === item.SendUserNickName}
        chatInfo={item}
        onLayout={onLayout}
        userPhoto={userPhoto}
        profilePress={() => setShowProfileBar(true)}
        key={index.toString()}
      />
    ))
  }, [chattingList, userPhoto])

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  //메세지 보내기 Callback function
  const sendMessage = useCallback(() => {
    if (chat.length > 0) {
      let data = new FormData()
      data.append('UserID', ID)
      data.append('UserNickname', Nickname)
      data.append(
        'OpponentID',
        chatRoomInfo.LastChat.SendUserID === ID
          ? chatRoomInfo.LastChat.ReceiveUserID
          : chatRoomInfo.LastChat.SendUserID
      )
      data.append('Message', chat)
      data.append('Photo', '')
      post('InsertChat.php', data)
        .then(() => {
          let data0 = new FormData()
          data0.append('Nickname', Nickname)
          data0.append('Content', chat)
          data0.append('Target_ID', chatRoomInfo.Opponent)
          post('InsertChattingAlert.php', data0).catch((e) => {
            setAlertMessage('채팅 푸시 알림 전송에 실패하였습니다.')
            setShowAlert(true)
          })
          setChat('')
        })
        .catch((e) => {
          setAlertMessage('채팅 전송에 실패하였습니다.')
          setShowAlert(true)
        })
    }
  }, [chat, ID, Nickname, chatRoomInfo])

  //사진 가져오기 Callback function
  const getPhoto = useCallback(() => {
    const options: ImageLibraryOptions = {
      mediaType: 'photo'
    }
    launchImageLibrary(options, (photo) => {
      photo?.assets?.map(({uri}) => setPhotoUri(uri!))
      sendPhoto()
    })
  }, [])

  //사진 보내기 Callback function
  const sendPhoto = useCallback(() => {
    let uploadData = new FormData()
    uploadData.append('submit', 'ok')
    uploadData.append('file', {
      type: 'image/jpeg',
      uri: photoUri,
      name: 'uploadimagetmp.jpg'
    })
    uploadData.append('target', ID)
    post('uploadImage.php', uploadData)
      .then((response) => response.json())
      .then((responseJson) => {
        if (responseJson.image.length > 0) {
          let data = new FormData()
          data.append('UserID', ID)
          data.append('UserNickname', Nickname)
          data.append(
            'OpponentID',
            chatRoomInfo.LastChat.SendUserID === ID
              ? chatRoomInfo.LastChat.ReceiveUserID
              : chatRoomInfo.LastChat.SendUserID
          )
          data.append('Message', '사진')
          data.append('Photo', responseJson.image)
          post('InsertChat.php', data)
            .then(() => {
              let data0 = new FormData()
              data0.append('Nickname', Nickname)
              data0.append('Content', '사진')
              data0.append('Target_ID', chatRoomInfo.Opponent)
              post('InsertChattingAlert.php', data0).catch((e) => {
                setAlertMessage('채팅 푸시 알림 전송에 실패하였습니다.')
                setShowAlert(true)
              })
              setPhotoUri('')
            })
            .catch((e) => {
              setAlertMessage('채팅 전송에 실패하였습니다.')
              setShowAlert(true)
            })
        }
      })
      .catch((e) => {
        setAlertMessage('이미지를 업로드하지 못했습니다.')
        setShowAlert(true)
      })
  }, [photoUri, ID, Nickname, chatRoomInfo])

  const blockUser = useCallback(() => {
    setShowBlockAlert(false)
    let data = new FormData()
    data.append('UserID', user.ID)
    data.append('Target', chatRoomInfo.Opponent)
    post('InsertBlock.php', data)
      .then(() => {
        setAlertMessage('차단이 완료되었습니다.')
        setShowAlert(true)
      })
      .catch((e) => {
        setAlertMessage('사용자 차단에 실패하였습니다.')
        setShowAlert(true)
      })
  }, [chatRoomInfo, user])

  const onLayout = useCallback(() => {
    scrollViewRef.current?.scrollToEnd()
  }, [])

  const reportPress = useCallback(() => {
    setShowProfileBar(false)
    setShowReportAlert(true)
  }, [])

  useEffect(() => {
    const interval_id = setInterval(() => {
      let data = new FormData()
      data.append('User', Nickname)
      data.append('Opponent', chatRoomInfo.Opponent)
      post('GetChattingRoom.php', data)
        .then((ChattingRoomInfo) => ChattingRoomInfo.json())
        .then((ChattingRoomInfoObject) => {
          const {GetChattingRoom} = ChattingRoomInfoObject
          setChattingList(GetChattingRoom)
        })
        .then(() => setChattingListLoading(false))
        .catch(() => {})
    }, 1000)
    return () => clearInterval(interval_id)
  }, [chatRoomInfo, Nickname])

  useEffect(() => {
    let data = new FormData()
    data.append('ID', chatRoomInfo.Opponent)
    post('GetUserNickName.php', data)
      .then((UserNickNameInfo) => UserNickNameInfo.json())
      .then((UserNickNameInfoObject) => {
        const {GetUserNickName} = UserNickNameInfoObject
        setOpponentNickName(GetUserNickName[0].Nickname)
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [chatRoomInfo])

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
    <>
      {loading && <Loading />}
      {!loading && (
        <SafeAreaView style={[styles.flex]}>
          <NavigationHeader
            viewStyle={{backgroundColor: 'white'}}
            title={opponentNickName}
            Left={() => (
              <Icon
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
            titleStyle={{fontWeight: 'bold'}}
          />
          <AutoFocusProvider
            contentContainerStyle={[styles.contentView]}
            keyboardShouldPersistTaps="always"
            scrollEnabled={false}>
            <ScrollView
              ref={scrollViewRef}
              onLayout={() => scrollViewRef.current?.scrollToEnd()}>
              {chatViewList}
            </ScrollView>
            <View style={[styles.sendchatview]}>
              <View
                style={[
                  {
                    alignItems: 'center',
                    justifyContent: 'center',
                    marginRight: 5
                  }
                ]}>
                <Icon
                  source={require('../assets/images/plus_black.png')}
                  size={40}
                  onPress={getPhoto}
                />
              </View>
              <TextInput
                style={[
                  styles.textInput,
                  {height: Platform.select({android: height})}
                ]}
                onFocus={focus}
                placeholder="채팅"
                value={chat}
                onChangeText={setChat}
                multiline={true}
                scrollEnabled={false}
                onContentSizeChange={(e) =>
                  setHeight(e.nativeEvent.contentSize.height)
                }
              />
              <View style={[{alignItems: 'center', justifyContent: 'center'}]}>
                <Icon
                  source={require('../assets/images/navigation.png')}
                  size={30}
                  onPress={sendMessage}
                />
              </View>
            </View>
          </AutoFocusProvider>
        </SafeAreaView>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
      {showProfileBar && (
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
