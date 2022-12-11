import React, {useCallback, useEffect, useState} from 'react'
import {StyleSheet, FlatList} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  AlertComponent,
  YoutubeView,
  Loading
} from '../components'
import {useNavigation} from '@react-navigation/native'
import {post_without_data} from '../server'
import type {YoutubeType} from '../type'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  }
})

export default function YoutubeRecommend() {
  const [youtubeInfoList, setYoutubeInfoList] = useState<YoutubeType[]>([])
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)
  const navigation = useNavigation()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  useEffect(() => {
    post_without_data('GetYoutubeInfo.php')
      .then((YoutubeInfo) => YoutubeInfo.json())
      .then((YoutubeInfoObject) => {
        const {GetYoutubeInfo} = YoutubeInfoObject
        setYoutubeInfoList(GetYoutubeInfo)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setAlertMessage('유튜브 영상 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setLoading(false)
      })
  }, [])

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
            title="자동차 추천 영상"
          />
          <FlatList
            data={youtubeInfoList}
            renderItem={({item}) => (
              <YoutubeView
                youtubeInfo={item}
                onPress={() =>
                  navigation.navigate('YoutubeWebView', {youtubeInfo: item})
                }
              />
            )}
            keyExtractor={(item, index) => index.toString()}
          />
        </SafeAreaView>
      )}
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
