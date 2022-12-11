import React, {useCallback, useState, useEffect, useMemo} from 'react'
import {
  Text,
  StyleSheet,
  View,
  TextInput,
  Dimensions,
  Pressable
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  TouchableView,
  Icon,
  AlertComponent,
  Loading
} from '../components'
import {useNavigation} from '@react-navigation/native'
import {useAutoFocus, AutoFocusProvider} from '../contexts'
import {useDispatch, useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import Color from 'color'
import {launchImageLibrary} from 'react-native-image-picker'
import type {ImageLibraryOptions} from 'react-native-image-picker'
import {post, serverUrl} from '../server'
import {
  changeBoolean,
  signOutWithKakao,
  writeToStorage,
  badword
} from '../utils'
import messaging from '@react-native-firebase/messaging'
import {GoogleSignin} from '@react-native-google-signin/google-signin'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  titleText: {
    fontSize: 22,
    color: '#191919',
    fontWeight: 'bold',
    marginLeft: 20,
    marginTop: 10
  },
  profileImageView: {
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: 30,
    marginBottom: 50
  },
  profileImage: {
    width: 100,
    height: 100,
    borderRadius: 50
  },
  cameraView: {
    position: 'absolute',
    width: 30,
    height: 30,
    borderRadius: 15,
    backgroundColor: '#433E50',
    bottom: 0,
    right: 0,
    alignItems: 'center',
    justifyContent: 'center'
  },
  autoFocusProvider: {
    flex: 1
  },
  textInputView: {
    width: Dimensions.get('window').width - 40,
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    marginLeft: 20,
    paddingVertical: 5,
    paddingHorizontal: 5,
    flexDirection: 'row',
    marginTop: 15
  },
  textInput: {
    flex: 1,
    fontSize: 16,
    color: '#191919',
    paddingVertical: 5
  },
  textInputButton: {
    width: 70,
    borderRadius: 5,
    alignItems: 'center',
    justifyContent: 'center'
  },
  textInputButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: '700'
  },
  okButton: {
    width: Dimensions.get('window').width - 40,
    height: 50,
    marginLeft: 20,
    backgroundColor: '#E6135A',
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: 50,
    borderRadius: 10
  },
  okButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: '700'
  },
  absoluteView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    backgroundColor: Color('#000000').alpha(0.45).string()
  },
  selectBar: {
    width: '100%',
    // height: 200,
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
  },
  checkText: {
    color: '#E6135A',
    marginLeft: 10,
    fontSize: 12,
    paddingVertical: 5,
    paddingHorizontal: 10
  }
})

export default function ChangeProfile() {
  const navigation = useNavigation()
  const focus = useAutoFocus()
  const dispatch = useDispatch()
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {Nickname, Photo, Email, ID, Login} = user
  //prettier-ignore
  const [photoSource, setPhotoSource] = useState(require('../assets/images/emptyProfileImage.png'))
  const [showSelectBar, setShowSelectBar] = useState<boolean>(false)
  const [changePhoto, setChangePhoto] = useState<string>('')
  const [editNickName, setEditNickName] = useState<string>(Nickname)
  const [editEmail, setEditEmail] = useState<string>(Email)
  //prettier-ignore
  const [nickNameOverLapCheck, setNickNameOverLapCheck] = useState<boolean>(false)
  const [badWordCheck, setBadWordCheck] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [withDrawAlert, setWithDrawAlert] = useState<boolean>(false)
  const [okLoading, setOKLoading] = useState<boolean>(false)

  const overLapCheckButton = useMemo(() => {
    if (nickNameOverLapCheck && badWordCheck) {
      return (
        <View style={[styles.textInputButton, {backgroundColor: '#01C73C'}]}>
          <Text style={[styles.textInputButtonText]}>체크완료</Text>
        </View>
      )
    } else if (Nickname === editNickName) {
      return (
        <View style={[styles.textInputButton, {backgroundColor: '#D9D9DF'}]}>
          <Text style={[styles.textInputButtonText]}>중복체크</Text>
        </View>
      )
    } else {
      return (
        <TouchableView
          style={[styles.textInputButton, {backgroundColor: '#E6135A'}]}
          onPress={() => OverLapCheckButtonClick()}>
          <Text style={[styles.textInputButtonText]}>중복체크</Text>
        </TouchableView>
      )
    }
  }, [Nickname, editNickName, nickNameOverLapCheck, badWordCheck])

  const okButton = useMemo(() => {
    if (
      (nickNameOverLapCheck && badWordCheck) ||
      Email !== editEmail ||
      changePhoto !== ''
    ) {
      return (
        <TouchableView
          style={[styles.okButton, {backgroundColor: '#E6135A'}]}
          onPress={() => okButtonClick()}>
          <Text style={[styles.okButtonText]}>확인</Text>
        </TouchableView>
      )
    } else {
      return (
        <View style={[styles.okButton, {backgroundColor: '#D9D9DF'}]}>
          <Text style={[styles.okButtonText]}>확인</Text>
        </View>
      )
    }
  }, [nickNameOverLapCheck, Email, editEmail, changePhoto, badWordCheck])

  const OverLapCheckButtonClick = () => {
    if (editNickName.length > 0) {
      let data = new FormData()
      data.append('CheckNickName', editNickName)
      post('NickNameOverLapCheck.php', data)
        .then((check) => check.json())
        .then((checkObject) => {
          const {NickNameOverLapCheck} = checkObject
          if (NickNameOverLapCheck[0].Count == 0) {
            setNickNameOverLapCheck(true)
            const badwordlist: string[] = badword.split('|')
            let i
            for (i = 0; i < badwordlist.length; i++) {
              if (editNickName.indexOf(badwordlist[i]) != -1) {
                setAlertMessage('닉네임에 불건전한 단어가 포함되어 있습니다.')
                setShowAlert(true)
                break
              }
            }
            if (i == badwordlist.length) {
              setBadWordCheck(true)
            }
          } else {
            setAlertMessage('이미 존재하는 닉네임입니다')
            setShowAlert(true)
          }
        })
        .catch((e) => {
          setAlertMessage('닉네임 중복체크 진행중 오류가 발생했습니다.')
          setShowAlert(true)
        })
    } else {
      setAlertMessage('닉네임을 입력해주세요')
      setShowAlert(true)
    }
  }

  const getUserInfo = () => {
    let data = new FormData()
    data.append('ID', ID)
    data.append('Login', Login)
    post('GetUserInfo.php', data)
      .then((userInfo) => userInfo.json())
      .then((userInfoObject) => {
        const {GetUserInfo} = userInfoObject
        const {
          Index_ID,
          ID,
          Login,
          Nickname,
          UserCar,
          Photo,
          Email,
          Phone,
          NoticeAlert,
          CommentAlert,
          LikeAlert,
          TagAlert,
          ChattingAlert,
          UserCarType
        } = GetUserInfo[0]
        const object: Ur.User = {
          Index_ID,
          ID,
          Login,
          Nickname,
          UserCar,
          Photo,
          Email,
          Phone,
          NoticeAlert: changeBoolean(NoticeAlert),
          CommentAlert: changeBoolean(CommentAlert),
          LikeAlert: changeBoolean(LikeAlert),
          TagAlert: changeBoolean(TagAlert),
          ChattingAlert: changeBoolean(ChattingAlert),
          UserCarType
        }
        dispatch(Ur.setUserAction(object))
      })
      .catch((e) => {
        setAlertMessage('사용자 정보를 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }

  const okButtonClick = () => {
    setOKLoading(true)
    if (nickNameOverLapCheck) {
      let data = new FormData()
      data.append('ID', ID)
      data.append('target', 'Nickname')
      data.append('value', editNickName)
      data.append('before', Nickname)
      post('UpdateUserInfo.php', data)
        .then(() => {
          getUserInfo()
        })
        .catch((e) => {
          setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
          setShowAlert(true)
          setOKLoading(false)
        })
    }
    if (Email !== editEmail) {
      let data = new FormData()
      data.append('ID', ID)
      data.append('target', 'Email')
      data.append('value', editEmail)
      data.append('before', Nickname)
      post('UpdateUserInfo.php', data)
        .then(() => {
          getUserInfo()
        })
        .catch((e) => {
          setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
          setShowAlert(true)
          setOKLoading(false)
        })
    }
    if (changePhoto !== '') {
      if (changePhoto !== 'delete') {
        let uploadData = new FormData()
        uploadData.append('submit', 'ok')
        uploadData.append('file', {
          type: 'image/jpeg',
          uri: photoSource.uri,
          name: 'uploadimagetmp.jpg'
        })
        uploadData.append('target', ID)
        post('uploadImage.php', uploadData)
          .then((response) => response.json())
          .then((responseJson) => {
            if (responseJson.image.length > 0) {
              let data = new FormData()
              data.append('ID', ID)
              data.append('target', 'Photo')
              data.append('value', responseJson.image)
              data.append('before', Nickname)
              post('UpdateUserInfo.php', data)
                .then(() => {
                  getUserInfo()
                })
                .catch((e) => {
                  setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
                  setShowAlert(true)
                  setOKLoading(false)
                })
            }
          })
          .catch((e) => {
            setAlertMessage('이미지를 업로드하지 못했습니다.')
            setShowAlert(true)
            setOKLoading(false)
          })
      } else {
        let data = new FormData()
        data.append('ID', ID)
        data.append('target', 'Photo')
        data.append('value', '')
        data.append('before', Nickname)
        post('UpdateUserInfo.php', data)
          .then(() => {
            getUserInfo()
          })
          .catch((e) => {
            setAlertMessage('사용자 정보를 업데이트하지 못했습니다.')
            setShowAlert(true)
            setOKLoading(false)
          })
      }
    }
    setPhotoSource(require('../assets/images/emptyProfileImage.png'))
    setChangePhoto('')
    setOKLoading(false)
    navigation.navigate('Home')
  }

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const getPhoto = useCallback(() => {
    const options: ImageLibraryOptions = {
      mediaType: 'photo'
    }
    launchImageLibrary(options, (photo) => {
      photo?.assets?.map(({uri}) => setPhotoSource({uri: uri!}))
      photo?.assets?.map(({uri}) => setChangePhoto(uri!))
    })
    setShowSelectBar(false)
  }, [])

  const deleteProfileImage = useCallback(() => {
    setPhotoSource(require('../assets/images/emptyProfileImage.png'))
    setChangePhoto('delete')
    setShowSelectBar(false)
  }, [])

  const nickNameOnChangeText = useCallback((text: string) => {
    setEditNickName(text)
    setNickNameOverLapCheck(false)
    setBadWordCheck(false)
  }, [])

  const deleteDeviceToken = useCallback(() => {
    messaging()
      .getToken()
      .then((token) => {
        let data = new FormData()
        data.append('Token', token)
        post('deleteDeviceInfo.php', data).catch((e) => {
          setAlertMessage('네트워크 연결에 실패했습니다.')
          setShowAlert(true)
        })
      })
      .catch((e) => {
        setAlertMessage('토큰을 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [])

  const withDraw = useCallback(() => {
    setWithDrawAlert(false)
    let data = new FormData()
    data.append('ID', ID)
    data.append('Nickname', Nickname)
    post('WithDraw.php', data).catch((e) => {
      setAlertMessage('사용자의 탈퇴가 정상적으로 처리되지 못했습니다.')
      setShowAlert(true)
    })
    if (Login === 'Kakao') {
      signOutWithKakao().catch((e) => {
        setAlertMessage('사용자의 로그아웃이 정상적으로 진행되지 않았습니다.')
        setShowAlert(true)
      })
    } else if (Login === 'Google') {
      GoogleSignin.configure()
      GoogleSignin.signOut().catch((e) => {
        setAlertMessage('사용자의 로그아웃이 정상적으로 진행되지 않았습니다.')
        setShowAlert(true)
      })
    }
    deleteDeviceToken()
    writeToStorage('loggedID', '')
      .then(() => {
        writeToStorage('loggedType', '')
          .then(() => {
            dispatch(Ur.distroyUserAction())
            navigation.reset({routes: [{name: 'Login'}]})
          })
          .catch((e) => {
            setAlertMessage(
              '사용자의 로그아웃이 정상적으로 진행되지 않았습니다.'
            )
            setShowAlert(true)
          })
      })
      .catch((e) => {
        setAlertMessage('사용자의 로그아웃이 정상적으로 진행되지 않았습니다.')
        setShowAlert(true)
      })
  }, [user])

  useEffect(() => {
    if (Photo.length > 0) {
      setPhotoSource({uri: serverUrl + 'img/' + Photo})
    }
  }, [user])

  return (
    <>
      {okLoading && <Loading />}
      {!okLoading && (
        <SafeAreaView style={[styles.view]}>
          <NavigationHeader
            viewStyle={[{backgroundColor: 'white'}]}
            Left={() => (
              <Icon
                source={require('../assets/images/arrow_left.png')}
                size={30}
                onPress={goBack}
              />
            )}
            Right={() => (
              <Text
                style={{color: '#767676'}}
                onPress={() => setWithDrawAlert(true)}>
                회원탈퇴
              </Text>
            )}
          />
          <AutoFocusProvider contentContainerStyle={[styles.autoFocusProvider]}>
            <Text style={[styles.titleText]}>내 정보 변경</Text>
            <View style={[styles.profileImageView]}>
              <TouchableView onPress={() => setShowSelectBar(true)}>
                <FastImage source={photoSource} style={[styles.profileImage]} />
                <View style={[styles.cameraView]}>
                  <FastImage
                    source={require('../assets/images/profileCamera.png')}
                    style={[{width: 20, height: 20}]}
                  />
                </View>
              </TouchableView>
            </View>
            <View style={[styles.textInputView]}>
              <TextInput
                onFocus={focus}
                style={[styles.textInput]}
                placeholder="닉네임을 입력해주세요"
                placeholderTextColor="#999999"
                value={editNickName}
                onChangeText={(text) => nickNameOnChangeText(text)}
              />
              {overLapCheckButton}
            </View>
            {(editNickName.length < 2 || editNickName.length > 8) && (
              <Text style={[styles.checkText]}>
                닉네임은 2~8자까지 사용가능합니다.
              </Text>
            )}
            <View style={[styles.textInputView]}>
              <TextInput
                onFocus={focus}
                style={[styles.textInput]}
                placeholder="이메일을 입력해주세요"
                placeholderTextColor="#999999"
                value={editEmail}
                onChangeText={setEditEmail}
              />
            </View>
            {okButton}
          </AutoFocusProvider>
        </SafeAreaView>
      )}
      {showSelectBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowSelectBar(false)}
          />
          <View style={[styles.selectBar]}>
            <TouchableView viewStyle={[styles.selectItem]} onPress={getPhoto}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/modify_background.png')}
              />
              <Text style={[styles.selectItemText]}>프로필 사진 수정하기</Text>
            </TouchableView>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={deleteProfileImage}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/delete_background.png')}
              />
              <Text style={[styles.selectItemText]}>삭제하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
      {withDrawAlert && (
        <AlertComponent
          message="정말 탈퇴를 하시겠습니까?"
          cancelUse={true}
          okPress={withDraw}
          cancelPress={() => setWithDrawAlert(false)}
        />
      )}
    </>
  )
}
