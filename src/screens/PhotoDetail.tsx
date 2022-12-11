import React, {useCallback} from 'react'
import {StyleSheet, Dimensions, View} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import AutoHeightImage from 'react-native-auto-height-image'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import type {ParamList} from '../navigator/MainNavigator'
import {serverUrl} from '../server'
import {NavigationHeader, Icon} from '../components'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    backgroundColor: 'white',
    flex: 1,
    alignItems: 'center'
  },
  image: {
    width: '100%',
    height: 200
  }
})

export default function PhotoDetail() {
  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'PhotoDetail'>>()
  const photo = route.params.photo

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  return (
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
      <View
        style={{
          flex: 1,
          alignItems: 'center',
          justifyContent: 'center',
          backgroundColor: 'black'
        }}>
        <FastImage
          style={{width: Dimensions.get('window').width, height: '100%'}}
          source={{uri: serverUrl + 'img/' + photo}}
          resizeMode="contain"
        />
      </View>
    </SafeAreaView>
  )
}
