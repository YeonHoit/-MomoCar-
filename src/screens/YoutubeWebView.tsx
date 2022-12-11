import React, {useCallback} from 'react'
import {StyleSheet} from 'react-native'
import {WebView} from 'react-native-webview'
import {SafeAreaView} from 'react-native-safe-area-context'
import {useRoute, useNavigation} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import {NavigationHeader, Icon} from '../components'
import type {ParamList} from '../navigator/MainNavigator'

const styles = StyleSheet.create({
  flex: {
    flex: 1
  }
})

export default function YoutubeWebView() {
  const route = useRoute<RouteProp<ParamList, 'YoutubeWebView'>>()
  const youtubeInfo = route.params.youtubeInfo
  const navigation = useNavigation()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  return (
    <SafeAreaView style={[styles.flex, {backgroundColor: 'white'}]}>
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
      <WebView
        style={[styles.flex]}
        mixedContentMode="always"
        source={{uri: youtubeInfo.url}}
        useWebKit={true}
        scrollEnabled={true}
        domStorageEnabled={true}
        javaScriptEnabled={true}
      />
    </SafeAreaView>
  )
}
