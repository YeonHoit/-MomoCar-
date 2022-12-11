import React from 'react'
import {createMaterialTopTabNavigator} from '@react-navigation/material-top-tabs'
import CafeList from '../screens/CafeList'
import RestList from '../screens/RestList'
import AttractionPlaceList from '../screens/AttractionPlaceList'
import DriveList from '../screens/DriveList'
import {Dimensions} from 'react-native'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import {Icon} from '../components'

const Tab = createMaterialTopTabNavigator()

const activeColor = '#E6135A'

export default function AttractionNavigator() {
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  return (
    <Tab.Navigator
      style={{backgroundColor: '#F8F8FA'}}
      tabBarOptions={{
        indicatorStyle: {
          opacity: 0
        },
        style: {
          width: Dimensions.get('window').width,
          shadowColor: 'white',
          backgroundColor: 'white',
          borderBottomLeftRadius: 20,
          borderBottomRightRadius: 20
        },
        tabStyle: {
          padding: 0
        },
        showLabel: false,
        showIcon: true,
        iconStyle: {
          width: 53,
          height: 53,
          marginBottom: 10
        }
      }}
      screenOptions={({route}) => ({
        tabBarIcon: ({focused, color}) => {
          let iconSource
          if (route.name === 'CafeList') {
            iconSource = focused
              ? require('../assets/images/cafe_fill.png')
              : require('../assets/images/cafe_empty.png')
          } else if (route.name === 'RestList') {
            iconSource = focused
              ? require('../assets/images/rest_fill.png')
              : require('../assets/images/rest_empty.png')
          } else if (route.name === 'AttractionPlaceList') {
            iconSource = focused
              ? require('../assets/images/attraction_fill.png')
              : require('../assets/images/attraction_empty.png')
          } else if (route.name === 'DriveList') {
            iconSource = focused
              ? require('../assets/images/drive_fill.png')
              : require('../assets/images/drive_empty.png')
          }
          return <Icon size={53} source={iconSource} />
        }
      })}
      lazy={true}>
      <Tab.Screen name="CafeList" component={CafeList} />
      <Tab.Screen name="RestList" component={RestList} />
      <Tab.Screen name="AttractionPlaceList" component={AttractionPlaceList} />
      <Tab.Screen name="DriveList" component={DriveList} />
    </Tab.Navigator>
  )
}
