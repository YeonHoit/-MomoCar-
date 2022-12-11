import React from 'react'
import {createMaterialTopTabNavigator} from '@react-navigation/material-top-tabs'
import {Icon} from '../components'
import type {RouteProp, ParamListBase} from '@react-navigation/native'
import {Dimensions, Platform} from 'react-native'
import Home from '../screens/Home'
import Community from '../screens/Community'
import TuningShop from '../screens/TuningShop'
import Alert from '../screens/Alert'
import {useSafeAreaInsets} from 'react-native-safe-area-context'
import UnityAlert from '../screens/UnityAlert'

type TabBarIconProps = {
  focused: boolean
  color: string
}

const icons: Record<string, any> = {
  Home: require('../assets/images/home.png'),
  Community: require('../assets/images/community.png'),
  TuningShop: require('../assets/images/tuningshop.png'),
  Alert: require('../assets/images/alert.png'),
  Simulation: require('../assets/images/gotuning.png')
}

const iconSize = Dimensions.get('window').width / 25

const screenOptions = ({route}: {route: RouteProp<ParamListBase, string>}) => {
  return {
    tabBarIcon: ({focused, color}: TabBarIconProps) => {
      const {name} = route
      const iconAddress = icons[name]
      return <Icon source={iconAddress} size={iconSize} />
    }
  }
}

const Tab = createMaterialTopTabNavigator()

export default function BottomTabNavigator() {
  const insets = useSafeAreaInsets()

  return (
    <Tab.Navigator
      tabBarPosition="bottom"
      swipeEnabled={false}
      tabBarOptions={{
        indicatorStyle: {
          backgroundColor: '#E6135A',
          marginBottom: insets.bottom
        },
        showIcon: true,
        style: {
          paddingBottom: insets.bottom,
          marginTop: -insets.bottom,
          backgroundColor: 'white',
          height: Platform.select({android: 60})
        },
        labelStyle: {
          fontSize: 10
        },
        iconStyle: {
          width: iconSize,
          height: iconSize
        }
      }}
      screenOptions={screenOptions}
      lazy={true}>
      <Tab.Screen
        name="Home"
        component={Home}
        options={{
          title: '홈'
        }}
      />
      <Tab.Screen
        name="Community"
        component={Community}
        options={{
          title: '커뮤니티'
        }}
      />
      {/* <Tab.Screen
        name="Simulation"
        component={UnityAlert}
        options={{
          title: '가상튜닝'
        }}
      /> */}
      <Tab.Screen
        name="TuningShop"
        component={TuningShop}
        options={{
          title: '튜닝샵'
        }}
      />
      <Tab.Screen
        name="Alert"
        component={Alert}
        options={{
          title: '알림'
        }}
      />
    </Tab.Navigator>
  )
}
