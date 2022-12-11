import React, {useEffect, useCallback, useState} from 'react'
import {StyleSheet} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {useNavigation} from '@react-navigation/native'
import {readFromStorage, changeBoolean, writeToStorage} from '../utils'
import {post} from '../server'
import * as Ur from '../store/user'
import {useDispatch} from 'react-redux'
import {AlertComponent} from '../components'
import FastImage from 'react-native-fast-image'
import AutoHeightImage from 'react-native-auto-height-image'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#E6135A'
  }
})

export default function Splash() {
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [showBanAlert, setShowBanAlert] = useState<boolean>(false)

  const navigation = useNavigation()
  const dispatch = useDispatch()

  const check = useCallback(() => {
    readFromStorage('loggedID')
      .then((res) => {
        if (res.length > 0) {
          readFromStorage('loggedType')
            .then((type) => {
              let data = new FormData()
              data.append('ID', res)
              data.append('Login', type)
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
                    dispatch(Ur.distroyUserAction())
                    writeToStorage('loggedID', '')
                    writeToStorage('loggedType', '')
                    setShowBanAlert(true)
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
                    dispatch(Ur.setUserAction(object))
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
                  setAlertMessage('네트워크를 확인해주세요.')
                  setShowAlert(true)
                })
            })
            .catch((e) => {
              setAlertMessage('네트워크를 확인해주세요.')
              setShowAlert(true)
            })
        } else {
          // navigation.navigate('Login')
          navigation.reset({routes: [{name: 'Login'}]})
        }
      })
      .catch((e) => {
        setAlertMessage('네트워크를 확인해주세요.')
        setShowAlert(true)
      })
  }, [])

  useEffect(() => {
    const id = setTimeout(() => check(), 3000)
    return () => clearTimeout(id)
  }, [])

  return (
    <>
      <SafeAreaView style={[styles.view]}>
        <AutoHeightImage
          width={80}
          source={require('../assets/images/appicon.png')}
        />
      </SafeAreaView>
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
      {showBanAlert && (
        <AlertComponent
          message="이용이 정지된 계정입니다."
          cancelUse={false}
          okPress={() => {
            setShowBanAlert(false)
            navigation.reset({routes: [{name: 'Login'}]})
          }}
        />
      )}
    </>
  )
}
