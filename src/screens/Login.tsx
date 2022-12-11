import React, {useCallback, useState} from 'react'
import {StyleSheet, Platform, View, Text} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {TouchableView, AlertComponent, Loading} from '../components'
import {
  signInWithKakao,
  getProfile,
  writeToStorage,
  changeBoolean,
  readFromStorage
} from '../utils'
import {useNavigation} from '@react-navigation/native'
import {useDispatch} from 'react-redux'
import * as Ur from '../store/user'
import {post, post_without_data} from '../server'
import {GoogleSignin} from '@react-native-google-signin/google-signin'
import messaging from '@react-native-firebase/messaging'
import * as RNFS from 'react-native-fs'
import type {UserJson} from '../type'
import FastImage from 'react-native-fast-image'
import {appleAuth} from '@invertase/react-native-apple-authentication'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'white'
  },
  touchableView: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 10
  },
  loginTextButton: {
    width: 300,
    height: 45,
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 1,
    borderColor: '#DBDBDB',
    borderRadius: 12
  },
  login_logo: {
    width: 200,
    height: 200,
    marginBottom: 100
  }
})

export default function Login() {
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)

  const navigation = useNavigation()
  const dispatch = useDispatch()

  GoogleSignin.configure()

  const saveJSON = (ID: string, UserCar: string) => {
    let temp: UserJson
    readFromStorage('userjson').then((value) => {
      if (value.length > 0) {
        let userjson: UserJson = JSON.parse(value)
        temp = {...userjson, ID, UserCar}
        writeToStorage('userjson', JSON.stringify(temp))
      } else {
        temp = {ID, UserCar, quality: 'H', frame: 'H'}
        writeToStorage('userjson', JSON.stringify(temp))
      }
    })
  }

  const saveDeviceToken = useCallback((id: string) => {
    messaging()
      .getToken()
      .then((token) => {
        let data = new FormData()
        data.append('ID', id)
        data.append('Token', token)
        post('InsertDeviceInfo.php', data).catch((e) => {
          setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
          setShowAlert(true)
        })
      })
      .catch((e) => {
        setAlertMessage('토큰을 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [])

  const KakaoLogin = useCallback(() => {
    signInWithKakao()
      .then(() => {
        getProfile()
          .then((profile) => {
            const {id} = profile
            let data = new FormData()
            data.append('ID', id)
            data.append('Login', 'Kakao')
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
                  UserCarType,
                  Ban
                } = GetUserInfo[0]
                if (changeBoolean(Ban)) {
                  setAlertMessage('이용이 정지된 계정입니다.')
                  setShowAlert(true)
                } else {
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
                  saveDeviceToken(id)
                  dispatch(Ur.setUserAction(object))
                  if (Platform.OS === 'ios') {
                    writeToStorage('loggedID', id.toString()).catch((e) => {
                      setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                      setShowAlert(true)
                    })
                  } else {
                    writeToStorage('loggedID', id).catch((e) => {
                      setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                      setShowAlert(true)
                    })
                  }
                  writeToStorage('loggedType', 'Kakao').catch((e) => {
                    setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                    setShowAlert(true)
                  })
                  //내부 저장소 JSON 파일 저장
                  // saveJSON(Index_ID, UserCar)
                  saveJSON(ID, UserCar)
                  if (!(Nickname.length > 0)) {
                    // navigation.navigate('Agreement')
                    navigation.reset({routes: [{name: 'Agreement'}]})
                  } else if (!(UserCar.length > 0)) {
                    // navigation.navigate('SetManufacturer')
                    navigation.reset({routes: [{name: 'SetManufacturer'}]})
                  } else {
                    // navigation.navigate('DrawerNavigator')
                    navigation.reset({routes: [{name: 'DrawerNavigator'}]})
                  }
                }
              })
              .catch((e) => {
                setAlertMessage('사용자의 정보를 불러오지 못했습니다.')
                setAlertMessage(e.message)
                setShowAlert(true)
              })
          })
          .catch((e) => {
            setAlertMessage('사용자의 정보를 불러오지 못했습니다.')
            setShowAlert(true)
          })
      })
      .catch((e) => {
        setAlertMessage('로그인 진행에 오류가 발생했습니다.')
        setShowAlert(true)
      })
  }, [])

  const GoogleLogin = useCallback(() => {
    GoogleSignin.hasPlayServices()
      .then(() => {
        GoogleSignin.signIn()
          .then((profile) => {
            const {user} = profile
            const {id} = user
            let data = new FormData()
            data.append('ID', id)
            data.append('Login', 'Google')
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
                  UserCarType,
                  Ban
                } = GetUserInfo[0]
                if (changeBoolean(Ban)) {
                  setAlertMessage('이용이 정지된 계정입니다.')
                  setShowAlert(true)
                } else {
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
                  saveDeviceToken(id)
                  dispatch(Ur.setUserAction(object))
                  writeToStorage('loggedID', id).catch((e) => {
                    setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                    setShowAlert(true)
                  })
                  writeToStorage('loggedType', 'Google').catch((e) => {
                    setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                    setShowAlert(true)
                  })
                  //내부 저장소 JSON 파일 저장
                  // saveJSON(Index_ID, UserCar)
                  saveJSON(ID, UserCar)
                  if (!(Nickname.length > 0)) {
                    // navigation.navigate('Agreement')
                    navigation.reset({routes: [{name: 'Agreement'}]})
                  } else if (!(UserCar.length > 0)) {
                    // navigation.navigate('SetManufacturer')
                    navigation.reset({routes: [{name: 'SetManufacturer'}]})
                  } else {
                    // navigation.navigate('DrawerNavigator')
                    navigation.reset({routes: [{name: 'DrawerNavigator'}]})
                  }
                }
              })
              .catch((e) => {
                setAlertMessage('사용자의 정보를 불러오지 못했습니다.')
                setShowAlert(true)
              })
          })
          .catch((e) => {
            setAlertMessage('사용자의 정보를 불러오지 못했습니다.')
            setShowAlert(true)
          })
      })
      .catch((e) => {
        setAlertMessage('로그인 진행에 오류가 발생했습니다.')
        setShowAlert(true)
      })
  }, [])

  const appleLogin = useCallback(() => {
    appleAuth
      .performRequest({
        requestedOperation: appleAuth.Operation.LOGIN,
        requestedScopes: [appleAuth.Scope.EMAIL]
      })
      .then((responseObject) => {
        appleAuth
          .getCredentialStateForUser(responseObject.user)
          .then((credentialState) => {
            if (credentialState === appleAuth.State.AUTHORIZED) {
              const id = responseObject.user
              let data = new FormData()
              data.append('ID', id)
              data.append('Login', 'Apple')
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
                    UserCarType,
                    Ban
                  } = GetUserInfo[0]
                  if (changeBoolean(Ban)) {
                    setAlertMessage('이용이 정지된 계정입니다.')
                    setShowAlert(true)
                  } else {
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
                    saveDeviceToken(id)
                    dispatch(Ur.setUserAction(object))
                    writeToStorage('loggedID', id).catch((e) => {
                      setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                      setShowAlert(true)
                    })
                    writeToStorage('loggedType', 'Google').catch((e) => {
                      setAlertMessage('네트워크 통신에 오류가 발생했습니다.')
                      setShowAlert(true)
                    })
                    //내부 저장소 JSON 파일 저장
                    // saveJSON(Index_ID, UserCar)
                    saveJSON(ID, UserCar)
                    if (!(Nickname.length > 0)) {
                      // navigation.navigate('Agreement')
                      navigation.reset({routes: [{name: 'Agreement'}]})
                    } else if (!(UserCar.length > 0)) {
                      // navigation.navigate('SetManufacturer')
                      navigation.reset({routes: [{name: 'SetManufacturer'}]})
                    } else {
                      // navigation.navigate('DrawerNavigator')
                      navigation.reset({routes: [{name: 'DrawerNavigator'}]})
                    }
                  }
                })
                .catch((e) => {
                  setAlertMessage('사용자의 정보를 불러오지 못했습니다.')
                  setShowAlert(true)
                })
            } else {
              setAlertMessage('로그인 진행에 오류가 발생했습니다.')
              setShowAlert(true)
            }
          })
          .catch((error) => {
            setAlertMessage('로그인 진행에 오류가 발생했습니다.')
            setShowAlert(true)
          })
      })
      .catch((error) => {
        setAlertMessage('로그인 진행에 오류가 발생했습니다.')
        setShowAlert(true)
      })
  }, [])

  const guestLogin = useCallback(() => {
    setLoading(true)
    post_without_data('UpdateGuestCount.php')
      .then(() => {
        const object: Ur.User = {
          Index_ID: -1,
          ID: 'guest',
          Login: 'guest',
          Nickname: '로그인',
          UserCar: '',
          Photo: '',
          Email: '',
          Phone: '',
          NoticeAlert: false,
          CommentAlert: false,
          LikeAlert: false,
          TagAlert: false,
          ChattingAlert: false,
          UserCarType: ''
        }
        dispatch(Ur.setUserAction(object))
        setLoading(false)
        navigation.reset({routes: [{name: 'DrawerNavigator'}]})
      })
      .catch((e) => {
        setLoading(false)
        setAlertMessage('둘러보기에 실패하였습니다.')
        setShowAlert(true)
      })
  }, [])

  return (
    <>
      {loading && <Loading />}
      <SafeAreaView style={[styles.view]}>
        <FastImage
          source={require('../assets/images/login_logo.png')}
          style={[styles.login_logo]}
        />
        <View>
          <TouchableView style={[styles.touchableView]} onPress={KakaoLogin}>
            <View style={[styles.loginTextButton]}>
              <FastImage
                source={require('../assets/images/kakao_login_button.png')}
                style={[{width: 204, height: 30}]}
              />
            </View>
          </TouchableView>
          <TouchableView style={[styles.touchableView]} onPress={GoogleLogin}>
            <View style={[styles.loginTextButton]}>
              <FastImage
                source={require('../assets/images/google_login_button.png')}
                style={[{width: 200, height: 28}]}
              />
            </View>
          </TouchableView>
          {Platform.OS === 'ios' && (
            <TouchableView style={[styles.touchableView]} onPress={appleLogin}>
              <View style={[styles.loginTextButton]}>
                <FastImage
                  source={require('../assets/images/apple_login_button.png')}
                  style={[{width: 200, height: 28}]}
                />
              </View>
            </TouchableView>
          )}
          <TouchableView style={[styles.touchableView]} onPress={guestLogin}>
            <View style={[styles.loginTextButton]}>
              <FastImage
                source={require('../assets/images/guest_login_button.png')}
                style={[{width: 200, height: 28}]}
              />
            </View>
          </TouchableView>
        </View>
      </SafeAreaView>
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
