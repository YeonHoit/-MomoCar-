import React from 'react'
import {createStackNavigator} from '@react-navigation/stack'
import type {ChatRoom, YoutubeType, Attraction} from '../type'
import Splash from '../screens/Splash'
import Login from '../screens/Login'
import EditNickName from '../screens/EditNickName'
import SetManufacturer from '../screens/SetManufacturer'
import SetCarType from '../screens/SetCarType'
import SetYearType from '../screens/SetYearType'
import DrawerNavigator from './DrawerNavigator'
import ChatRoomScreen from '../screens/ChatRoomScreen'
import ChangeProfile from '../screens/ChangeProfile'
import ChangeManufacturer from '../screens/ChangeManufacturer'
import WritePost from '../screens/WritePost'
import PostDetailView from '../screens/PostDetailView'
import YoutubeWebView from '../screens/YoutubeWebView'
import AlertSetting from '../screens/AlertSetting'
import ServiceInfo from '../screens/ServiceInfo'
import gaein from '../clause/gaein'
import Agreement from '../screens/Agreement'
import userClause from '../clause/userClause'
import locateClause from '../clause/locateClause'
import YoutubeRecommend from '../screens/YoutubeRecommend'
import Unity from '../screens/Unity'
import UnityModal from '../screens/UnityModal'
import ModifyPost from '../screens/ModifyPost'
import OtherThingScreen from '../screens/OtherThingScreen'
import ReviewScreen from '../screens/ReviewScreen'
import WriteReview from '../screens/WriteReview'
import PhotoDetail from '../screens/PhotoDetail'
import AttractionScreen from '../screens/AttractionScreen'
import AttractionDetailScreen from '../screens/AttractionDetailScreen'
import WriteAttractionReview from '../screens/WriteAttractionReview'

export type ParamList = {
  Splash: {}
  Login: {}
  EditNickName: {}
  SetManufacturer: {}
  SetCarType: {
    manufacturer: string
  }
  SetYearType: {
    carType: string
  }
  DrawerNavigator: {}
  ChatRoomScreen: {
    chatRoomInfo: ChatRoom
  }
  ChangeProfile: {}
  ChangeManufacturer: {}
  WritePost: {
    boardName: string
  }
  PostDetailView: {
    // communityNumber: number
    communityNumber: string
  }
  YoutubeWebView: {
    youtubeInfo: YoutubeType
  }
  AlertSetting: {}
  ServiceInfo: {}
  gaein: {}
  userClause: {}
  locateClause: {}
  Agreement: {}
  YoutubeRecommend: {}
  Unity: {}
  UnityModal: {}
  ModifyPost: {
    communityNumber: number
  }
  OtherThingScreen: {
    communityNumber: string
    NickName: string
  }
  ReviewScreen: {
    NickName: string
  }
  WriteReview: {
    NickName: string
  }
  PhotoDetail: {
    photo: string
  }
  AttractionScreen: {}
  AttractionDetailScreen: {
    attractionInfo: Attraction
  }
  WriteAttractionReview: {
    Target: number
  }
}

const Stack = createStackNavigator<ParamList>()

export default function MainNavigator() {
  return (
    <Stack.Navigator screenOptions={{headerShown: false}}>
      <Stack.Screen name="Splash" component={Splash} />
      <Stack.Screen
        name="Login"
        component={Login}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen
        name="EditNickName"
        component={EditNickName}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen
        name="SetManufacturer"
        component={SetManufacturer}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen name="SetCarType" component={SetCarType} />
      <Stack.Screen name="SetYearType" component={SetYearType} />
      <Stack.Screen
        name="DrawerNavigator"
        component={DrawerNavigator}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen name="ChatRoomScreen" component={ChatRoomScreen} />
      <Stack.Screen name="ChangeProfile" component={ChangeProfile} />
      <Stack.Screen name="ChangeManufacturer" component={ChangeManufacturer} />
      <Stack.Screen name="WritePost" component={WritePost} />
      <Stack.Screen
        name="PostDetailView"
        component={PostDetailView}
        getId={({params}) => params.communityNumber}
      />
      <Stack.Screen name="YoutubeWebView" component={YoutubeWebView} />
      <Stack.Screen name="AlertSetting" component={AlertSetting} />
      <Stack.Screen name="ServiceInfo" component={ServiceInfo} />
      <Stack.Screen name="gaein" component={gaein} />
      <Stack.Screen name="userClause" component={userClause} />
      <Stack.Screen name="locateClause" component={locateClause} />
      <Stack.Screen
        name="Agreement"
        component={Agreement}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen name="YoutubeRecommend" component={YoutubeRecommend} />
      <Stack.Screen
        name="Unity"
        component={Unity}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen
        name="UnityModal"
        component={UnityModal}
        options={{gestureEnabled: false}}
      />
      <Stack.Screen name="ModifyPost" component={ModifyPost} />
      <Stack.Screen
        name="OtherThingScreen"
        component={OtherThingScreen}
        getId={({params}) => params.communityNumber}
      />
      <Stack.Screen name="ReviewScreen" component={ReviewScreen} />
      <Stack.Screen name="WriteReview" component={WriteReview} />
      <Stack.Screen name="PhotoDetail" component={PhotoDetail} />
      <Stack.Screen name="AttractionScreen" component={AttractionScreen} />
      <Stack.Screen
        name="AttractionDetailScreen"
        component={AttractionDetailScreen}
      />
      <Stack.Screen
        name="WriteAttractionReview"
        component={WriteAttractionReview}
      />
    </Stack.Navigator>
  )
}
