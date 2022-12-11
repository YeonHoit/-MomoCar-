import React, {useCallback, useState, useMemo, useEffect} from 'react'
import {
  StyleSheet,
  Text,
  TextInput,
  Dimensions,
  View,
  Platform,
  Pressable,
  TouchableOpacity,
  Modal
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  TouchableView,
  AlertComponent,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import {AutoFocusProvider, useAutoFocus} from '../contexts'
import RNPickerSelect from 'react-native-picker-select'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {post, serverUrl} from '../server'
import Color from 'color'
import {launchImageLibrary} from 'react-native-image-picker'
import type {ImageLibraryOptions} from 'react-native-image-picker'
import type {ParamList} from '../navigator/MainNavigator'
import FastImage from 'react-native-fast-image'
import * as Progress from 'react-native-progress'

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
  contentsInput: {
    fontSize: 16,
    width: Dimensions.get('window').width - 40,
    marginLeft: 20,
    marginTop: 10,
    color: '#191919'
  },
  selectImage: {
    width: Dimensions.get('window').width - 40,
    marginLeft: 20,
    flexDirection: 'row',
    flexWrap: 'wrap'
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
  absoluteView: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    backgroundColor: Color('#000000').alpha(0.45).string()
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

export default function ModifyPost() {
  const route = useRoute<RouteProp<ParamList, 'ModifyPost'>>()
  const communityNumber = route.params.communityNumber
  const [selectBoard, setSelectBoard] = useState<string>('')
  const [title, setTitle] = useState<string>('')
  const [price, setPrice] = useState<string>('')
  const [contents, setContents] = useState<string>('')
  const [image, setImage] = useState<string[]>([])
  const [originalPhoto, setOriginalPhoto] = useState<string>('')
  const [showUpdateBar, setShowUpdateBar] = useState<boolean>(false)
  const [pickPhoto, setPickPhoto] = useState<number>(-1)
  const [showFourAlert, setShowFourAlert] = useState<boolean>(false)
  const [errorMessage, setErrorMessage] = useState<string>('')
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false)
  const [showModifyAlert, setShowModifyAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)
  const [modifyLoading, setModifyLoading] = useState<boolean>(false)
  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const focus = useAutoFocus()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const urlImage = useMemo(() => {
    return image.map((item) => {
      if (item.includes('file:')) {
        return item
      } else {
        return serverUrl + 'img/' + item
      }
    })
  }, [image])

  const selectPhoto = useMemo(() => {
    return urlImage.map((item, index) => (
      <TouchableOpacity
        key={index.toString()}
        onPress={() => {
          setPickPhoto(index)
          console.log(index)
          setShowUpdateBar(true)
        }}>
        <FastImage
          source={{uri: item}}
          style={{
            width: (Dimensions.get('window').width - 60) / 2,
            height: 100,
            marginRight: 10,
            marginBottom: 10
          }}
        />
      </TouchableOpacity>
    ))
  }, [image])

  const deletePhoto = useCallback(() => {
    let temp: string[] = [...image]
    temp.splice(pickPhoto, 1)
    setImage(temp)
  }, [pickPhoto, image])

  const modifyPhoto = useCallback(() => {
    let temp: string[] = [...image]
    const options: ImageLibraryOptions = {
      mediaType: 'photo'
    }
    launchImageLibrary(options, (photo) => {
      photo?.assets?.map(({uri}) => (temp[pickPhoto] = uri!))
      setImage(temp)
    })
  }, [pickPhoto, image])

  const getPhoto = useCallback(() => {
    if (image.length == 4) {
      setShowFourAlert(true)
      return
    }
    const options: ImageLibraryOptions = {
      mediaType: 'photo'
    }
    launchImageLibrary(options, (photo) => {
      photo?.assets?.map(({uri}) => setImage([...image, uri!]))
    })
  }, [image])

  const upload = (): Promise<string> => {
    return new Promise(async (resolve) => {
      let serverPhoto = ''
      if (urlImage.length != 0) {
        let i: number = 0
        for await (let item of urlImage) {
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
          if (urlImage.length == i + 1) {
            resolve(serverPhoto)
          }
          i += 1
        }
      }
    })
  }

  const uploadImage = useCallback(() => {
    upload().then((serverPhoto) => {
      console.log(serverPhoto)
      modifyPost(serverPhoto)
    })
  }, [urlImage, user])

  const seperateFunc = useCallback(() => {
    setShowModifyAlert(false)
    setModifyLoading(true)
    if (urlImage.length != 0) {
      uploadImage()
    } else {
      modifyPost('')
    }
  }, [urlImage, title, contents, selectBoard, user])

  const modifyPost = useCallback(
    (serverPhoto: string) => {
      let data = new FormData()
      data.append('CommunityNumber', communityNumber)
      data.append('BoardName', selectBoard)
      data.append('Title', title)
      data.append('Contents', contents)
      data.append('Photo', serverPhoto)
      data.append('Price', selectBoard === 'UsedMarket' ? price : 0)
      post('UpdateCommunityPost.php', data).catch((e) => {
        setErrorMessage('글을 업로드하는데 실패했습니다.')
        setShowErrorAlert(true)
      })
      setModifyLoading(false)
      navigation.canGoBack() && navigation.goBack()
    },
    [communityNumber, selectBoard, title, contents, price]
  )

  const writeCheck = useCallback(() => {
    if (selectBoard == null) {
      setErrorMessage('게시판을 선택해주세요.')
      setShowErrorAlert(true)
    } else if (selectBoard.length == 0) {
      setErrorMessage('게시판을 선택해주세요.')
      setShowErrorAlert(true)
    } else if (title.length == 0) {
      setErrorMessage('제목을 작성해주세요.')
      setShowErrorAlert(true)
    } else if (contents.length == 0) {
      setErrorMessage('내용을 작성해주세요.')
      setShowErrorAlert(true)
    } else {
      setShowModifyAlert(true)
    }
  }, [selectBoard, title, contents])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    post('GetCommunityPost.php', data)
      .then((CommunityPostInfo) => CommunityPostInfo.json())
      .then((CommunityPostInfoObject) => {
        const {GetCommunityPost} = CommunityPostInfoObject
        const {BoardName, Title, Contents, Photo, Price} = GetCommunityPost[0]
        setSelectBoard(BoardName)
        setTitle(Title)
        setPrice(Price)
        setContents(Contents)
        let temp: string[] = Photo.split('|')
        temp.splice(temp.length - 1, 1)
        setImage(temp)
        setOriginalPhoto(Photo)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setErrorMessage('글 정보를 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setLoading(false)
      })
  }, [communityNumber])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
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
                수정완료
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
      )}
      <Pressable style={[styles.floatingButton]} onPress={getPhoto}>
        <FastImage
          source={require('../assets/images/photo_floating.png')}
          style={[{width: 60, height: 60}]}
        />
      </Pressable>
      {showUpdateBar && (
        <View style={[styles.absoluteView]}>
          <Pressable
            style={[{flex: 1}]}
            onPress={() => setShowUpdateBar(false)}
          />
          <View style={[styles.selectBar]}>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setShowUpdateBar(false)
                modifyPhoto()
              }}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/modify_background.png')}
              />
              <Text style={[styles.selectItemText]}>수정하기</Text>
            </TouchableView>
            <TouchableView
              viewStyle={[styles.selectItem]}
              onPress={() => {
                setShowUpdateBar(false)
                deletePhoto()
              }}>
              <FastImage
                style={[{width: 35, height: 35}]}
                source={require('../assets/images/delete_background.png')}
              />
              <Text style={[styles.selectItemText]}>삭제하기</Text>
            </TouchableView>
          </View>
        </View>
      )}
      {showFourAlert && (
        <AlertComponent
          message="사진은 최대 4장까지 첨부가능합니다."
          cancelUse={false}
          okPress={() => setShowFourAlert(false)}
        />
      )}
      {showErrorAlert && (
        <AlertComponent
          message={errorMessage}
          cancelUse={false}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
      {showModifyAlert && (
        <AlertComponent
          message="글을 수정하시겠습니까?"
          cancelUse={true}
          okPress={seperateFunc}
          cancelPress={() => setShowModifyAlert(false)}
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
