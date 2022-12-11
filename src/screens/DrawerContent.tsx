import React, {useCallback, useState, useMemo} from 'react'
import {StyleSheet, View, Text, Pressable} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {NavigationHeader, Icon, AlertComponent} from '../components'
import type {FC} from 'react'
import type {DrawerContentComponentProps} from '@react-navigation/drawer'
import {DrawerActions} from '@react-navigation/native'
import {useSelector, useDispatch} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {signOutWithKakao, writeToStorage} from '../utils'
import {GoogleSignin} from '@react-native-google-signin/google-signin'
import messaging from '@react-native-firebase/messaging'
import {post, serverUrl} from '../server'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    padding: 5,
    backgroundColor: 'white'
  },
  content: {
    flex: 1
  },
  text: {
    fontSize: 17,
    marginBottom: 20,
    fontWeight: '500'
  },
  profileImage: {
    width: 70,
    height: 70,
    borderRadius: 35
  },
  profileImageView: {
    flexDirection: 'row',
    marginLeft: 20,
    alignItems: 'center',
    marginTop: 20
  }
})

const DrawerContent: FC<DrawerContentComponentProps> = (props) => {
  const {navigation} = props
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {Login, Nickname} = user
  const dispatch = useDispatch()
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)

  const close = useCallback(() => {
    navigation.dispatch(DrawerActions.closeDrawer())
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

  const logout = useCallback(() => {
    navigation.dispatch(DrawerActions.closeDrawer())
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
  }, [Login])

  const profileImage = useMemo(() => {
    if (user.Photo.length > 0) {
      return {uri: serverUrl + 'img/' + user.Photo}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [user])

  const profilePress = useCallback(() => {
    if (user.ID === 'guest') {
      navigation.reset({routes: [{name: 'Login'}]})
    }
  }, [user])

  return (
    <>
      <SafeAreaView style={[styles.view]}>
        <NavigationHeader
          Right={() => (
            <Icon
              source={require('../assets/images/close.png')}
              size={18}
              onPress={close}
            />
          )}
        />
        <View style={[styles.content]}>
          <Pressable style={[styles.profileImageView]} onPress={profilePress}>
            <FastImage source={profileImage} style={[styles.profileImage]} />
            <Text style={[{fontSize: 18, marginLeft: 10}]}>{Nickname}</Text>
          </Pressable>
          <View style={[{marginLeft: 20, marginTop: 50}]}>
            {user.ID !== 'guest' && (
              <Text
                style={[styles.text]}
                onPress={() => navigation.navigate('ChangeProfile')}>
                내 정보 변경
              </Text>
            )}
            {user.ID !== 'guest' && (
              <Text
                style={[styles.text]}
                onPress={() => navigation.navigate('ChangeManufacturer')}>
                내 차종 변경
              </Text>
            )}
            {user.ID !== 'guest' && (
              <Text
                style={[styles.text]}
                onPress={() => navigation.navigate('AlertSetting')}>
                알림 설정
              </Text>
            )}
            <Text
              style={[styles.text]}
              onPress={() => navigation.navigate('ServiceInfo')}>
              서비스 정보
            </Text>
          </View>
          <View
            style={[
              {
                flex: 1,
                alignItems: 'flex-end',
                justifyContent: 'flex-end',
                flexDirection: 'row'
              }
            ]}>
            {user.ID !== 'guest' && (
              <Text
                style={[
                  {
                    color: '#767676',
                    marginBottom: 10,
                    marginRight: 10,
                    fontSize: 12
                  }
                ]}
                onPress={logout}>
                로그아웃 {'>'}
              </Text>
            )}
            {user.ID === 'guest' && (
              <Text
                style={{
                  color: '#767676',
                  marginBottom: 10,
                  marginRight: 10,
                  fontSize: 12
                }}
                onPress={profilePress}>
                로그인 {'>'}
              </Text>
            )}
          </View>
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

export default DrawerContent
