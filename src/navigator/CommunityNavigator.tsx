import React from 'react'
import {createMaterialTopTabNavigator} from '@react-navigation/material-top-tabs'
import UserCarCommunity from '../screens/UserCarCommunity'
import FreeBoardCommunity from '../screens/FreeBoardCommunity'
import UsedMarketCommunity from '../screens/UsedMarketCommunity'
import Chatting from '../screens/Chatting'
import MyPosting from '../screens/MyPosting'
import {Dimensions} from 'react-native'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'

const Tab = createMaterialTopTabNavigator()

const activeColor = '#E6135A'

export default function CommunityNavigator() {
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  return (
    <Tab.Navigator
      tabBarOptions={{
        labelStyle: {fontSize: 14, fontWeight: '700'},
        indicatorStyle: {
          backgroundColor: activeColor
        },
        activeTintColor: activeColor,
        inactiveTintColor: '#767676',
        style: {
          width: Dimensions.get('window').width - 40,
          marginLeft: 20,
          shadowColor: 'white'
        },
        tabStyle: {padding: 0}
      }}
      lazy={true}>
      {user.ID !== 'guest' && (
        <Tab.Screen
          name="UserCarCommunity"
          component={UserCarCommunity}
          options={{title: user.UserCarType}}
        />
      )}
      <Tab.Screen
        name="FreeBoardCommunity"
        component={FreeBoardCommunity}
        options={{title: '자유'}}
      />
      <Tab.Screen
        name="UsedMarketCommunity"
        component={UsedMarketCommunity}
        options={{title: '중고장터'}}
      />
      {user.ID !== 'guest' && (
        <Tab.Screen
          name="Chatting"
          component={Chatting}
          options={{title: '채팅'}}
        />
      )}
      {user.ID !== 'guest' && (
        <Tab.Screen
          name="MyPosting"
          component={MyPosting}
          options={{title: '내가쓴글'}}
        />
      )}
    </Tab.Navigator>
  )
}
