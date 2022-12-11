import React, {useCallback, useMemo, useState} from 'react'
import {
  StyleSheet,
  Text,
  Dimensions,
  View,
  Pressable,
  TextInput,
  Modal
} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  TouchableView,
  TouchableImage,
  AlertComponent
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import FastImage from 'react-native-fast-image'
import {AutoFocusProvider, useAutoFocus} from '../contexts'
import type {ParamList} from '../navigator/MainNavigator'
import {launchImageLibrary} from 'react-native-image-picker'
import type {ImageLibraryOptions} from 'react-native-image-picker'
import AutoHeightImage from 'react-native-auto-height-image'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import Color from 'color'
import * as Progress from 'react-native-progress'
import {post} from '../server'

const styles = StyleSheet.create({
  view: {
    backgroundColor: 'white',
    flex: 1
  },
  scrollView: {
    paddingHorizontal: 20,
    width: '100%'
  },
  completeButton: {
    backgroundColor: '#E6135A',
    width: Dimensions.get('window').width - 40,
    height: 50,
    alignItems: 'center',
    justifyContent: 'center',
    marginLeft: 20,
    marginVertical: 20,
    borderRadius: 10
  },
  star: {
    width: 30,
    height: 30
  },
  devideLine: {
    backgroundColor: '#EDEDED',
    width: Dimensions.get('window').width - 40,
    height: 1,
    marginTop: 20
    // marginBottom: 20
  },
  textInput: {
    width: Dimensions.get('window').width - 40
  },
  addImage: {
    width: Dimensions.get('window').width - 40,
    minHeight: 100
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

export default function WriteAttractionReview() {
  const [starCount, setStarCount] = useState<number>(1)
  const [image, setImage] = useState<string[]>([])
  const [content, setContent] = useState<string>('')
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [showWriteAlert, setShowWriteAlert] = useState<boolean>(false)
  const [writeLoading, setWriteLoading] = useState<boolean>(false)

  const navigation = useNavigation()
  const focus = useAutoFocus()
  const route = useRoute<RouteProp<ParamList, 'WriteAttractionReview'>>()
  const Target = route.params.Target
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const stars = useMemo(() => {
    let check: boolean[] = [false, false, false, false, false]
    if (starCount == 1) {
      check = [true, false, false, false, false]
    } else if (starCount == 2) {
      check = [true, true, false, false, false]
    } else if (starCount == 3) {
      check = [true, true, true, false, false]
    } else if (starCount == 4) {
      check = [true, true, true, true, false]
    } else if (starCount == 5) {
      check = [true, true, true, true, true]
    }
    return check.map((bool, index) => {
      if (bool) {
        return (
          <Pressable
            style={[styles.star, {marginRight: 10}]}
            key={index.toString()}
            onPress={() => setStarCount(index + 1)}>
            <FastImage
              style={[styles.star]}
              source={require('../assets/images/star_fill.png')}
            />
          </Pressable>
        )
      }
      return (
        <Pressable
          style={[styles.star, {marginRight: 10}]}
          key={index.toString()}
          onPress={() => setStarCount(index + 1)}>
          <FastImage
            style={[styles.star]}
            source={require('../assets/images/star_empty.png')}
          />
        </Pressable>
      )
    })
  }, [starCount])

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

  const photoList = useMemo(() => {
    return image.map((item, index) => (
      <AutoHeightImage
        width={Dimensions.get('window').width - 40}
        source={{uri: item}}
        key={index.toString()}
        style={{marginBottom: 20}}
      />
    ))
  }, [image])

  const writeCheck = useCallback(() => {
    if (content.length == 0) {
      setAlertMessage('후기 내용을 작성해주세요.')
      setShowAlert(true)
    } else {
      setShowWriteAlert(true)
    }
  }, [content])

  const seperateFunc = useCallback(() => {
    if (image.length != 0) {
      setShowWriteAlert(false)
      setWriteLoading(true)
      uploadImage()
    } else {
      setShowWriteAlert(false)
      setWriteLoading(true)
      writeReview('')
    }
  }, [image])

  const uploadImage = useCallback(() => {
    upload().then((serverPhoto) => {
      writeReview(serverPhoto)
    })
  }, [image, user])

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

  const writeReview = useCallback(
    (serverPhoto: string) => {
      setShowAlert(false)
      let data = new FormData()
      data.append('Target', Target)
      data.append('WriterID', user.ID)
      data.append('Star', starCount)
      data.append('Contents', content)
      data.append('Photo', serverPhoto)
      post('InsertAttractionReview.php', data).catch((e) => {
        setAlertMessage('글을 업로드하는데 실패했습니다.')
        setShowAlert(true)
      })
      setWriteLoading(false)
      navigation.goBack()
    },
    [Target, user, starCount, content]
  )

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
        />
        <AutoFocusProvider
          contentContainerStyle={[styles.scrollView]}
          showsVerticalScrollIndicator={false}>
          <Text>별점을 남겨주세요.</Text>
          <View style={{flexDirection: 'row', marginTop: 10}}>{stars}</View>
          <View style={[styles.devideLine]} />
          <TextInput
            placeholder="후기 내용"
            style={[styles.textInput]}
            multiline={true}
            onFocus={focus}
            value={content}
            onChangeText={(value) => setContent(value)}
          />
          {photoList}
          {image.length != 4 && (
            <TouchableImage
              style={[styles.addImage]}
              source={require('../assets/images/photo_add.png')}
              resizeMode="contain"
              onPress={getPhoto}
            />
          )}
        </AutoFocusProvider>
        <TouchableView viewStyle={[styles.completeButton]} onPress={writeCheck}>
          <Text style={{color: 'white'}}>완료</Text>
        </TouchableView>
      </SafeAreaView>
      {showAlert && (
        <AlertComponent
          cancelUse={false}
          message={alertMessage}
          okPress={() => setShowAlert(false)}
        />
      )}
      {showWriteAlert && (
        <AlertComponent
          cancelUse={true}
          message="후기를 작성하시겠습니까?"
          cancelPress={() => setShowWriteAlert(false)}
          okPress={seperateFunc}
        />
      )}
      {writeLoading && (
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
