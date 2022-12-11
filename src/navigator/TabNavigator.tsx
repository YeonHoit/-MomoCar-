import React from 'react'
import {createBottomTabNavigator} from '@react-navigation/bottom-tabs'
import {Colors} from 'react-native-paper'
import Icon from 'react-native-vector-icons/MaterialCommunityIcons'
import type {RouteProp, ParamListBase} from '@react-navigation/native'
import Home from '../screens/Home'
import Community from '../screens/Community'
import TuningShop from '../screens/TuningShop'
import Alert from '../screens/Alert'

type TabBarIconProps = {
  focused: boolean
  color: string
  size: number
}

const activeColor = Colors.redA200

const icons: Record<string, string[]> = {
  Home: ['home-circle', 'home-circle-outline'],
  Community: ['clipboard-text-multiple', 'clipboard-text-multiple-outline'],
  TuningShop: ['store', 'store-outline'],
  Alert: ['bell', 'bell-outline']
}

const screenOptions = ({route}: {route: RouteProp<ParamListBase, string>}) => {
  return {
    tabBarIcon: ({focused, color, size}: TabBarIconProps) => {
      const {name} = route
      const focusedSize = focused ? size + 3 : size
      const focusedColor = focused ? activeColor : color
      const [icon, iconOutline] = icons[name]
      const iconName = focused ? icon : iconOutline
      return <Icon name={iconName} size={focusedSize} color={focusedColor} />
    }
  }
}

const Tab = createBottomTabNavigator()

export default function TabNavigator() {
  return (
    <Tab.Navigator
      screenOptions={screenOptions}
      tabBarOptions={{activeTintColor: activeColor}}>
      <Tab.Screen name="Home" component={Home} options={{title: '홈'}} />
      <Tab.Screen
        name="Community"
        component={Community}
        options={{title: '커뮤니티'}}
      />
      <Tab.Screen
        name="TuningShop"
        component={TuningShop}
        options={{title: '튜닝샵'}}
      />
      <Tab.Screen name="Alert" component={Alert} options={{title: '알림'}} />
    </Tab.Navigator>
  )
}
