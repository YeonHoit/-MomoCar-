import React, {useCallback} from 'react'
import {StyleSheet, Text} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {NavigationHeader, Icon} from '../components'
import {useNavigation, DrawerActions} from '@react-navigation/native'
import CommunityNavigator from '../navigator/CommunityNavigator'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  text: {
    fontSize: 20
  }
})

export default function Community() {
  const navigation = useNavigation()

  const drawerOpen = useCallback(() => {
    navigation.dispatch(DrawerActions.openDrawer())
  }, [])

  return (
    <SafeAreaView style={[styles.view]}>
      <NavigationHeader
        viewStyle={[{backgroundColor: 'white'}]}
        Left={() => (
          <Text style={{fontSize: 16, fontWeight: 'bold'}}>커뮤니티</Text>
        )}
        Right={() => (
          <Icon
            source={require('../assets/images/menu.png')}
            size={30}
            onPress={drawerOpen}
          />
        )}
      />
      <CommunityNavigator />
    </SafeAreaView>
  )
}
