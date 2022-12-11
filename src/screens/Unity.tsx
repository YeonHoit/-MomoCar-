import React, {useEffect, useCallback} from 'react'
import {StyleSheet, View, BackHandler, Platform} from 'react-native'
import UnityView, {UnityModule} from '@asmadsen/react-native-unity-view'
import Orientation from 'react-native-orientation-locker'
import {useNavigation} from '@react-navigation/native'
import {readFromStorage, writeToStorage} from '../utils'
import {UserJson} from '../type'

const styles = StyleSheet.create({
  view: {
    flex: 1
  }
})

export default function Unity() {
  const navigation = useNavigation()

  const backpress = () => {
    return true
  }

  const request_user_json = useCallback(() => {
    readFromStorage('userjson').then((value) => {
      if (value.length > 0) {
        console.log('response : ' + value)
        UnityModule.postMessageToUnityManager('response_user_json|' + value)
      }
    })
  }, [])

  const save_user_json = useCallback((data: string) => {
    let temp = JSON.parse(data)
    let userjsontemp: UserJson
    let userjson: UserJson
    readFromStorage('userjson').then((value) => {
      if (value.length > 0) {
        userjsontemp = JSON.parse(value)
        userjson = {
          ...userjsontemp,
          quality: temp.quality,
          frame: temp.frame
        }
        console.log('save : ' + JSON.stringify(userjson))
        writeToStorage('userjson', JSON.stringify(userjson))
      }
    })
  }, [])

  useEffect(() => {
    Orientation.unlockAllOrientations()
    if (Platform.OS === 'ios') {
      Orientation.lockToLandscapeLeft()
    }
    UnityModule.isReady().then((value) => {
      if (value) {
        UnityModule.postMessageToUnityManager('start|')
      }
    })
    BackHandler.addEventListener('hardwareBackPress', backpress)
    return () => {
      BackHandler.removeEventListener('hardwareBackPress', backpress)
      Orientation.unlockAllOrientations()
      Orientation.lockToPortrait()
    }
  }, [])

  return (
    <View style={[styles.view]}>
      <View style={[styles.view]}>
        <UnityView
          style={{
            flex: 1
          }}
          onMessage={(value) => {
            console.log(value)
            // if (value === 'exit') {
            //   navigation.canGoBack() && navigation.goBack()
            // }
            let valueArray: string[] = value.split('|')
            let order: string = valueArray[0]
            if (order === 'exit') {
              navigation.canGoBack() && navigation.goBack()
            } else if (order === 'request_user_json') {
              request_user_json()
            } else if (order === 'save_user_json') {
              save_user_json(valueArray[1])
            }
          }}
        />
      </View>
    </View>
  )
}
