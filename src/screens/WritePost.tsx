import React, {useState, useCallback, useMemo} from 'react'
import {
  StyleSheet,
  Text,
  TextInput,
  Dimensions,
  Pressable,
  View,
  Platform,
  Modal
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {NavigationHeader, Icon, AlertComponent} from '../components'
import RNPickerSelect from 'react-native-picker-select'
import {AutoFocusProvider, useAutoFocus} from '../contexts'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {ParamList} from '../navigator/MainNavigator'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post} from '../server'
import {launchImageLibrary} from 'react-native-image-picker'
import type {ImageLibraryOptions} from 'react-native-image-picker'
import FastImage from 'react-native-fast-image'
import * as Progress from 'react-native-progress'
import Color from 'color'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  completeText: {
    fontSize: 14,
    color: '#767676',
    fontWeight: 'bold'
  },
  titleInput: {
    borderBottomWidth: 1,
    borderBottomColor: '#DBDBDB',
    marginLeft: 20,
    width: Dimensions.get('window').width - 40,
    marginTop: 20,
    fontSize: 18,
    paddingVertical: 5,
    color: 'black',
    fontWeight: 'bold'
  },
  contentsInput: {
    fontSize: 16,
    width: Dimensions.get('window').width - 40,
    marginLeft: 20,
    marginTop: 10,
    color: '#191919'
  },
  floatingButton: {
    position: 'absolute',
    bottom: 60,
    right: 20,
    width: 60,
    height: 60,
    borderRadius: 30,
    alignItems: 'center',
    justifyContent: 'center'
  },
  priceView: {
    marginLeft: 20,
    marginTop: 10,
    paddingVertical: 10,
    flexDirection: 'row',
    alignItems: 'center'
  },
  wonText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'black'
  },
  priceTextInput: {
    fontSize: 18,
    marginLeft: 10,
    color: '#767676'
  },
  selectImage: {
    width: Dimensions.get('window').width - 40,
    marginLeft: 20,
    flexDirection: 'row',
    flexWrap: 'wrap'
  },
  modalView: {
    flex: 1,
    position: 'absolute',
    width: '100%',
    height: '100%',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: Color('#000000').alpha(0.3).string()
  }
})

const pickerSelectStyles = StyleSheet.create({
  inputIOS: {
    fontSize: 14,
    height: 40,
    width: 130,
    color: 'white',
    padding: 10,
    marginLeft: 20,
    backgroundColor: '#E6135A',
    borderRadius: 10,
    fontWeight: 'bold',
    // paddingLeft: 15,
    textAlign: 'center'
  },
  inputAndroid: {
    fontSize: 14,
    height: 40,
    width: 130,
    color: 'white',
    padding: 10,
    marginLeft: 20,
    backgroundColor: '#E6135A',
    borderRadius: 10,
    fontWeight: 'bold',
    // paddingLeft: 15,
    textAlign: 'center'
  },
  placeholder: {
    color: 'white',
    fontSize: 14
  },
  viewContainer: {
    width: 140,
    height: 40,
    marginTop: 10,
    alignItems: 'center'
  },
  iconContainer: {
    top: 4
  }
})

export default function WritePost() {
  const route = useRoute<RouteProp<ParamList, 'WritePost'>>()
  const boardName = route.params.boardName

  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false)
  const [errorMessage, setErrorMessage] = useState<string>('')
  const [showCompleteAlert, setShowCompleteAlert] = useState<boolean>(false)
  const [selectBoard, setSelectBoard] = useState<string>(boardName)
  const [cancelUse, setCancelUse] = useState<boolean>(false)
  const [title, setTitle] = useState<string>('')
  const [contents, setContents] = useState<string>('')
  const [price, setPrice] = useState<string>('')
  const [image, setImage] = useState<string[]>([])
  const [modifyLoading, setModifyLoading] = useState<boolean>(false)
  const focus = useAutoFocus()
  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const writeCheck = useCallback(() => {
    if (selectBoard == null) {
      setCancelUse(false)
      setAlertMessage('게시판을 선택해주세요.')
      setShowAlert(true)
    } else if (selectBoard.length == 0) {
      setCancelUse(false)
      setAlertMessage('게시판을 선택해주세요.')
      setShowAlert(true)
    } else if (title.length == 0) {
      setCancelUse(false)
      setAlertMessage('제목을 작성해주세요.')
      setShowAlert(true)
    } else if (contents.length == 0) {
      setCancelUse(false)
      setAlertMessage('내용을 작성해주세요.')
      setShowAlert(true)
    } else {
      setCancelUse(true)
      setAlertMessage('글을 작성하시겠습니까?')
      setShowAlert(true)
    }
  }, [selectBoard, title, contents])

  const upload = (): Promise<string> => {
    return new Promise(async (resolve) => {
      let serverPhoto = ''
      if (image.length != 0) {
        let i: number = 0
        for await (let item of image) {
          let uploadData = new FormData()
          uploadData.append('submit', 'ok')
          uploadData.append('file', {
            type: 'image/jpeg',
            uri: item,
            name: 'uploadimagetmp.jpg'
          })
          uploadData.append('target', user.ID)
          uploadData.append('Number', i + 1)
          const response = await post('uploadCommunityImage.php', uploadData)
          const responseJson = await response.json()
          serverPhoto = serverPhoto + responseJson.image + '|'
          if (image.length == i + 1) {
            resolve(serverPhoto)
          }
          i += 1
        }
      }
    })
  }

  const uploadImage = useCallback(() => {
    upload().then((serverPhoto) => {
      writePost(serverPhoto)
    })
  }, [image, user, title, contents, selectBoard])

  const writePost = useCallback(
    (serverPhoto: string) => {
      setShowAlert(false)
      let data = new FormData()
      data.append('BoardName', selectBoard)
      data.append('NickName', user.Nickname)
      data.append('Title', title)
      data.append('Contents', contents)
      data.append('Photo', serverPhoto)
      data.append('Price', selectBoard === 'UsedMarket' ? price : 0)
      post('InsertCommunityPost.php', data).catch((e) => {
        setErrorMessage('글을 업로드하는데 실패했습니다.')
        setShowErrorAlert(true)
      })
      setModifyLoading(false)
      setShowCompleteAlert(true)
    },
    [selectBoard, title, contents, price, user.Nickname]
  )

  const seperateFunc = useCallback(() => {
    if (!cancelUse) {
      setShowAlert(false)
    } else if (image.length != 0) {
      setShowAlert(false)
      setModifyLoading(true)
      uploadImage()
    } else {
      setShowAlert(false)
      setModifyLoading(true)
      writePost('')
    }
  }, [cancelUse, image])

  const getPhoto = useCallback(() => {
    if (image.length == 4) {
      setAlertMessage('사진은 최대 4장까지 첨부가능합니다.')
      setShowAlert(true)
      return
    }
    const options: ImageLibraryOptions = {
      mediaType: 'photo'
    }
    launchImageLibrary(options, (photo) => {
      photo?.assets?.map(({uri}) => setImage([...image, uri!]))
    })
  }, [image])

  const selectPhoto = useMemo(() => {
    return image.map((item, index) => (
      <FastImage
        source={{uri: item}}
        key={index.toString()}
        style={{
          width: (Dimensions.get('window').width - 60) / 2,
          height: 100,
          marginRight: 10,
          marginBottom: 10
        }}
      />
    ))
  }, [image])

  return (
    <>
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
            <Text style={[styles.completeText]} onPress={writeCheck}>
              작성완료
            </Text>
          )}
        />
        <AutoFocusProvider contentContainerStyle={[{flex: 1}]}>
          <RNPickerSelect
            textInputProps={{underlineColorAndroid: 'transparent'}}
            placeholder={{label: '게시판 선택'}}
            fixAndroidTouchableBug={true}
            value={selectBoard}
            onValueChange={(value) => setSelectBoard(value)}
            useNativeAndroidPickerStyle={false}
            items={[
              {
                label: user.UserCarType,
                value: user.UserCarType,
                key: user.UserCarType
              },
              {label: '자유 게시판', value: 'FreeBoard', key: '자유 게시판'},
              {label: '중고장터', value: 'UsedMarket', key: '중고장터'}
            ]}
            style={pickerSelectStyles}
          />
          <TextInput
            style={[styles.titleInput]}
            onFocus={focus}
            placeholder="글 제목"
            value={title}
            onChangeText={(value) => setTitle(value)}
            placeholderTextColor="#767676"
          />
          {selectBoard === 'UsedMarket' && (
            <View style={[styles.priceView]}>
              <Text style={[styles.wonText]}>￦</Text>
              <TextInput
                style={[styles.priceTextInput]}
                onFocus={focus}
                placeholder="금액"
                value={price}
                onChangeText={setPrice}
                keyboardType={
                  Platform.OS === 'android' ? 'number-pad' : 'numeric'
                }
                placeholderTextColor="#767676"
              />
            </View>
          )}
          <TextInput
            style={[styles.contentsInput]}
            onFocus={focus}
            placeholder="글 내용"
            value={contents}
            onChangeText={(value) => setContents(value)}
            placeholderTextColor="#767676"
            multiline={true}
          />
          <View style={[styles.selectImage]}>{selectPhoto}</View>
        </AutoFocusProvider>
      </SafeAreaView>
      <Pressable style={[styles.floatingButton]} onPress={getPhoto}>
        <FastImage
          source={require('../assets/images/photo_floating.png')}
          style={[{width: 60, height: 60}]}
        />
      </Pressable>
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={cancelUse}
          okPress={seperateFunc}
          cancelPress={() => setShowAlert(false)}
        />
      )}
      {showErrorAlert && (
        <AlertComponent
          message={errorMessage}
          cancelUse={false}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
      {showCompleteAlert && (
        <AlertComponent
          message="글 작성이 완료되었습니다."
          cancelUse={false}
          okPress={() => {
            setShowCompleteAlert(false)
            navigation.navigate('Community')
          }}
        />
      )}
      {modifyLoading && (
        <Modal transparent={true}>
          <View style={[styles.modalView]}>
            <Progress.CircleSnail
              color={['#E6135A']}
              size={40}
              duration={1000}
              spinDuration={4000}
              thickness={4}
            />
          </View>
        </Modal>
      )}
    </>
  )
}
