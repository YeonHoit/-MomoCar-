import React from 'react'
import {createDrawerNavigator} from '@react-navigation/drawer'
import TabNavigator from './TabNavigator'
import DrawerContent from '../screens/DrawerContent'
import BottomTabNavigator from './BottomTabNavigator'

const Drawer = createDrawerNavigator()

export default function DrawerNavigator() {
  return (
    <Drawer.Navigator
      drawerContent={(props) => <DrawerContent {...props} />}
      drawerPosition="right"
      lazy={true}>
      <Drawer.Screen name="TabNavigator" component={BottomTabNavigator} />
    </Drawer.Navigator>
  )
}
