import 'react-native-gesture-handler'
import React, {useState, useCallback, useEffect, useRef} from 'react'
import {enableScreens} from 'react-native-screens'
import {SafeAreaProvider} from 'react-native-safe-area-context'
import {
  NavigationContainer,
  DefaultTheme,
  DarkTheme
} from '@react-navigation/native'
import {AppearanceProvider, useColorScheme} from 'react-native-appearance'
import {Provider as ReduxProvider} from 'react-redux'
import {ToggleThemeProvider} from './src/contexts'
import MainNavigator from './src/navigator/MainNavigator'
import {makeStore} from './src/store'
import messaging from '@react-native-firebase/messaging'
import {
  View,
  StyleSheet,
  Text,
  Dimensions,
  Image,
  StatusBar,
  Platform
} from 'react-native'
import Orientation from 'react-native-orientation-locker'
import * as RNFS from 'react-native-fs'
import {Loading} from './src/components'
import SplashScreen from 'react-native-splash-screen'

const styles = StyleSheet.create({
  view: {
    position: 'absolute',
    width: Dimensions.get('window').width - 16,
    borderRadius: 20,
    top: 40,
    left: 8,
    borderWidth: 1,
    backgroundColor: 'white',
    borderColor: '#EEEEEE',
    paddingVertical: 10,
    paddingLeft: 10,
    flexDirection: 'row'
  },
  image: {
    width: 50,
    height: 50
  },
  titleText: {
    fontSize: 15,
    flex: 1
  },
  contentsText: {
    fontSize: 15,
    color: '#767676'
  }
})

enableScreens()

const store = makeStore()

export default function App() {
  const scheme = useColorScheme()
  const [theme, setTheme] = useState(DefaultTheme)
  const [title, setTitle] = useState<string>('')
  const [contents, setContents] = useState<string>('')
  const [show, toggleShow] = useState<boolean>(false)
  const [exitApp, setExitApp] = useState<boolean>(false)
  const [timeOutID, setTimeOutID] = useState<NodeJS.Timeout>()

  if (Platform.OS === 'ios') {
    StatusBar.setBarStyle('dark-content')
  } else if (Platform.OS === 'android') {
    StatusBar.setBarStyle('light-content')
  }

  const toggleTheme = useCallback(
    () => setTheme(({dark}) => (dark ? DefaultTheme : DarkTheme)),
    []
  )

  const foregroundListener = useCallback(() => {
    messaging().onMessage(async (message) => {
      console.log('message: ', message)
      setTitle(message.notification?.title!)
      setContents(message.notification?.body!)
      toggleShow(false)
      toggleShow(true)
      setTimeout(() => toggleShow(false), 5000)
    })
  }, [])

  useEffect(() => {
    if (Platform.OS === 'ios') {
      SplashScreen.hide()
    }
    foregroundListener()
    if (Platform.OS === 'ios') {
      Orientation.lockToPortrait()
    }
  }, [])

  return (
    <>
      <AppearanceProvider>
        <ToggleThemeProvider toggleTheme={toggleTheme}>
          <SafeAreaProvider>
            <ReduxProvider store={store}>
              <NavigationContainer theme={theme}>
                <MainNavigator />
              </NavigationContainer>
            </ReduxProvider>
          </SafeAreaProvider>
        </ToggleThemeProvider>
      </AppearanceProvider>
      {show && (
        <View style={[styles.view]}>
          <Image
            source={require('./src/assets/images/alert_notice.png')}
            style={[styles.image]}
          />
          <View style={{marginLeft: 10, flex: 1}}>
            <View
              style={{
                flexDirection: 'row',
                marginBottom: 5
              }}>
              <Text style={[styles.titleText]}>{title}</Text>
            </View>
            <Text style={[styles.contentsText]}>{contents}</Text>
          </View>
        </View>
      )}
    </>
  )
}
