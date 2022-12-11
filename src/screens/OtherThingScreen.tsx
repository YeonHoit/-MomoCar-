import React, {useCallback, useEffect, useState, useMemo} from 'react'
import {StyleSheet, ScrollView} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  Icon,
  AlertComponent,
  OtherThingDetail,
  Loading
} from '../components'
import {useNavigation, useRoute} from '@react-navigation/native'
import type {RouteProp} from '@react-navigation/native'
import {post} from '../server'
import type {CommunityPost} from '../type'
import type {ParamList} from '../navigator/MainNavigator'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  headerView: {
    width: '100%',
    paddingHorizontal: 20,
    paddingVertical: 5,
    marginBottom: 10
  },
  headerText: {
    fontSize: 14,
    fontWeight: 'bold'
  }
})

export default function OtherThingScreen() {
  const [otherThingList, setOtherThingList] = useState<CommunityPost[]>([])
  const [errorMessage, setErrorMessage] = useState<string>('')
  const [showErrorAlert, setShowErrorAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)
  const navigation = useNavigation()
  const route = useRoute<RouteProp<ParamList, 'OtherThingScreen'>>()
  const communityNumberTrash = route.params.communityNumber
  const communityNumber = communityNumberTrash.split('|')[0]
  const NickName = route.params.NickName

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  const otherThingComponent = useMemo(() => {
    return otherThingList.map((item, index) => (
      <OtherThingDetail
        communityPostInfo={item}
        indexNumber={index}
        key={index.toString()}
        onPress={() =>
          navigation.navigate('PostDetailView', {
            communityNumber:
              item.Index_num.toString() + '|' + new Date().toString()
          })
        }
      />
    ))
  }, [otherThingList])

  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityNumber)
    data.append('NickName', NickName)
    post('GetOtherThing.php', data)
      .then((OtherThingInfo) => OtherThingInfo.json())
      .then((OtherThingInfoObject) => {
        const {GetOtherThing} = OtherThingInfoObject
        setOtherThingList(GetOtherThing)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setErrorMessage('판매 내역 목록을 불러오지 못했습니다.')
        setShowErrorAlert(true)
        setLoading(false)
      })
  }, [communityNumber, NickName])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
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
            title={`${NickName}님의 상품`}
          />
          <ScrollView
            contentContainerStyle={{
              backgroundColor: 'white',
              flexDirection: 'row',
              flexWrap: 'wrap'
            }}>
            {otherThingComponent}
          </ScrollView>
        </SafeAreaView>
      )}
      {showErrorAlert && (
        <AlertComponent
          cancelUse={false}
          message={errorMessage}
          okPress={() => setShowErrorAlert(false)}
        />
      )}
    </>
  )
}
