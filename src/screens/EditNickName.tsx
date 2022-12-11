import React, {useState, useCallback, useMemo} from 'react'
import {StyleSheet, Text, View, TextInput} from 'react-native'
import {TouchableView, AlertComponent} from '../components'
import {SafeAreaView} from 'react-native-safe-area-context'
import {useNavigation} from '@react-navigation/native'
import {useAutoFocus, AutoFocusProvider} from '../contexts'
import {useDispatch, useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post} from '../server'
import RNPickerSelect from 'react-native-picker-select'
import FastImage from 'react-native-fast-image'
import {badword} from '../utils'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'white'
  },
  keyboardAwareFocus: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center'
  },
  text: {
    fontSize: 20
  },
  textInput: {
    padding: 10,
    fontSize: 15,
    height: 60,
    flex: 1,
    backgroundColor: '#F8F8FA',
    color: 'black'
  },
  touchableView: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center'
  },
  textInputButtonView: {
    flexDirection: 'row',
    width: '90%',
    marginVertical: 5,
    alignItems: 'center'
    // marginBottom: 400
  },
  editNickName_logo: {
    width: 200,
    height: 200
  },
  checkText: {
    color: '#E6135A',
    marginLeft: 10,
    fontSize: 12
  }
})

const pickerSelectStyles = StyleSheet.create({
  inputIOS: {
    fontSize: 16,
    height: 60,
    width: 80,
    color: 'black',
    borderColor: 'black',
    borderWidth: 1,
    padding: 10
  },
  inputAndroid: {
    fontSize: 16,
    height: 60,
    width: 80,
    color: 'black',
    borderColor: 'black',
    borderWidth: 1,
    padding: 10
  }
})

export default function EditNickName() {
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const {ID} = user
  const [editNickName, setEditNickName] = useState<string>('')
  const [editRealName, setEditRealName] = useState<string>('')
  const [editPhoneNumber, setEditPhoneNumber] = useState<string>('')
  const [selectedCarrier, setSelectedCarrier] = useState<string>('')
  const [overLapCheck, setOverLapCheck] = useState<boolean>(false)
  const [badWordCheck, setBadWordCheck] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)

  const focus = useAutoFocus()
  const navigation = useNavigation()
  const dispatch = useDispatch()

  const totalCheck = useMemo(() => {
    if (
      overLapCheck &&
      badWordCheck &&
      editNickName.length >= 2 &&
      editNickName.length <= 8
    ) {
      return true
    } else {
      return false
    }
  }, [overLapCheck, badWordCheck, editNickName])

  const EditNickName_onClick = useCallback(() => {
    if (editNickName.length > 0 && totalCheck) {
      let data = new FormData()
      data.append('ID', ID)
      data.append('target', 'Nickname')
      data.append('value', editNickName)
      post('UpdateUserInfo.php', data)
        .then(() => {
          dispatch(Ur.setUserAction({...user, Nickname: editNickName}))
          // navigation.navigate('SetManufacturer')
          navigation.reset({routes: [{name: 'SetManufacturer'}]})
        })
        .catch((e) => {
          setAlertMessage('사용자의 정보가 업데이트되지 못했습니다.')
          setShowAlert(true)
        })
    } else if (editNickName.length == 0) {
      setAlertMessage('닉네임을 입력해주세요')
      setShowAlert(true)
    } else {
      setAlertMessage('닉네임 중복 확인을 해주세요')
      setShowAlert(true)
    }
  }, [editNickName, ID, totalCheck])

  const changeNickname = useCallback((nickname: string) => {
    setOverLapCheck(false)
    setBadWordCheck(false)
    setEditNickName(nickname)
  }, [])

  const nicknameCheck = useCallback(() => {
    if (editNickName.length > 0) {
      let data = new FormData()
      data.append('CheckNickName', editNickName)
      post('NickNameOverLapCheck.php', data)
        .then((check) => check.json())
        .then((checkObject) => {
          const {NickNameOverLapCheck} = checkObject
          if (NickNameOverLapCheck[0].Count == 0) {
            setOverLapCheck(true)
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
          setAlertMessage('닉네임 체크에 오류가 발생했습니다.')
          setShowAlert(true)
        })
    } else {
      setAlertMessage('닉네임을 입력해주세요')
      setShowAlert(true)
    }
  }, [editNickName])

  const navigateButton = useMemo(() => {
    if (totalCheck) {
      return (
        <TouchableView
          style={[
            styles.touchableView,
            {
              backgroundColor: '#E6135A',
              width: '90%',
              height: 50,
              borderRadius: 10,
              marginBottom: 20
            }
          ]}
          onPress={EditNickName_onClick}>
          <Text style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
            가입하기
          </Text>
        </TouchableView>
      )
    } else {
      return (
        <View
          style={[
            styles.touchableView,
            {
              backgroundColor: '#D9D9DF',
              width: '90%',
              height: 50,
              borderRadius: 10,
              marginBottom: 20
            }
          ]}>
          <Text style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
            가입하기
          </Text>
        </View>
      )
    }
  }, [totalCheck])

  return (
    <>
      <SafeAreaView style={[styles.view]}>
        <AutoFocusProvider contentContainerStyle={[styles.keyboardAwareFocus]}>
          <FastImage
            source={require('../assets/images/login_logo.png')}
            style={[styles.editNickName_logo]}
          />
          <View style={{marginBottom: 400}}>
            <View style={[styles.textInputButtonView]}>
              <TextInput
                onFocus={focus}
                style={[
                  styles.textInput,
                  totalCheck && {
                    borderBottomWidth: 2,
                    borderColor: '#01C73C'
                  }
                ]}
                value={editNickName}
                onChangeText={(nickname) => changeNickname(nickname)}
                placeholder="닉네임을 입력해주세요"
                placeholderTextColor="#767676"
              />
              <TouchableView
                style={[
                  styles.touchableView,
                  {width: 80, height: 60},
                  !totalCheck && {backgroundColor: '#E6135A'},
                  totalCheck && {backgroundColor: '#01C73C'}
                ]}
                onPress={nicknameCheck}>
                {!totalCheck && (
                  <Text
                    style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
                    중복{'\n'}확인
                  </Text>
                )}
                {totalCheck && (
                  <Text
                    style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
                    체크{'\n'}완료
                  </Text>
                )}
              </TouchableView>
            </View>
            {(editNickName.length < 2 || editNickName.length > 8) && (
              <Text style={[styles.checkText]}>
                닉네임은 2~8자까지 사용가능합니다.
              </Text>
            )}
          </View>
          {/* <View style={[styles.textInputButtonView]}>
            <TextInput
              onFocus={focus}
              style={[styles.textInput]}
              value={editRealName}
              onChangeText={setEditRealName}
              placeholder="이름 입력"
            />
          </View>
          <View style={[styles.textInputButtonView]}>
            <View style={[{width: 80, height: 60}]}>
              <RNPickerSelect
                textInputProps={{underlineColorAndroid: 'transparent'}}
                placeholder={{label: '통신사'}}
                fixAndroidTouchableBug={true}
                value={selectedCarrier}
                onValueChange={(value) => setSelectedCarrier(value)}
                useNativeAndroidPickerStyle={false}
                items={[
                  {label: 'SKT', value: 'SKT', key: 'SKT'},
                  {label: 'KT', value: 'KT', key: 'KT'},
                  {label: 'LGU+', value: 'LGU+', key: 'LGU+'}
                ]}
                style={pickerSelectStyles}
              />
            </View>
            <TextInput
              onFocus={focus}
              style={[styles.textInput, {flex: 1}]}
              value={editPhoneNumber}
              onChangeText={setEditPhoneNumber}
              placeholder="전화번호 입력"
            />
            <TouchableView
              style={[
                styles.touchableView,
                {backgroundColor: Colors.redA200, width: 60, height: 60}
              ]}>
              <Text>인증</Text>
            </TouchableView>
          </View> */}
          {navigateButton}
        </AutoFocusProvider>
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
