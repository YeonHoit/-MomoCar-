import React, {useCallback, useEffect, useState} from 'react'
import {StyleSheet, View, FlatList, Text, RefreshControl} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {
  NavigationHeader,
  AlertView,
  AlertComponent,
  Loading
} from '../components'
import {useNavigation} from '@react-navigation/native'
import {post} from '../server'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import type {AlertType} from '../type'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  }
})

// 푸시 알림 리스트 화면
export default function Alert() {
  const [alertList, setAlertList] = useState<AlertType[]>([]) // 푸시 알림 리스트 배열
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false) // 새로고침 진행중 여부
  const [alertMessage, setAlertMessage] = useState<string>('') // 알림창 메세지 문자열
  const [showAlert, setShowAlert] = useState<boolean>(false) // 알림창 표시 여부
  const [alertListLoading, setAlertListLoading] = useState<boolean>(true) // 알림 리스트 로딩 여부

  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  // 푸시 알림 클릭 이벤트 함수
  const noticeClick = useCallback((alertInfo: AlertType) => {
    if (alertInfo.Target_num != -1) {
      navigation.navigate('PostDetailView', {
        communityNumber: alertInfo.Target_num
      })
    }
  }, [])

  // 리스트 새로 고침 함수
  const refresh = useCallback(() => {
    setIsRefreshing(true)
    let data = new FormData()
    data.append('User', user.Nickname)
    post('GetAlertInfo.php', data)
      .then((AlertInfo) => AlertInfo.json())
      .then((AlertInfoObject) => {
        const {GetAlertInfo} = AlertInfoObject
        setAlertList(GetAlertInfo)
      })
      .then(() => {
        setIsRefreshing(false)
      })
      .catch((e) => {
        setAlertMessage('알림 목록을 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [user])

  // 푸시 알림 리스트 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('User', user.Nickname)
    post('GetAlertInfo.php', data)
      .then((AlertInfo) => AlertInfo.json())
      .then((AlertInfoObject) => {
        const {GetAlertInfo} = AlertInfoObject
        setAlertList(GetAlertInfo)
      })
      .then(() => setAlertListLoading(false))
      .catch((e) => {
        setAlertMessage('알림 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setAlertListLoading(false)
      })
  }, [user])

  return (
    <>
      {/* 로딩중일 때 */}
      {alertListLoading && <Loading />}
      {/* 로딩중이 아닐 때 */}
      {!alertListLoading && (
        <SafeAreaView style={[styles.view]}>
          {/* 화면 상단 바 */}
          <NavigationHeader
            Left={() => (
              <Text style={{fontSize: 16, fontWeight: 'bold'}}>알림</Text>
            )}
            viewStyle={{backgroundColor: 'white'}}
          />
          {/* 푸시 알림 리스트 */}
          <View style={{flex: 1, backgroundColor: '#F8F8FA'}}>
            <FlatList
              data={alertList}
              renderItem={({item}) => (
                <AlertView alertInfo={item} onPress={() => noticeClick(item)} />
              )}
              keyExtractor={(item, index) => index.toString()}
              showsVerticalScrollIndicator={false}
              refreshControl={
                <RefreshControl
                  refreshing={isRefreshing}
                  onRefresh={refresh}
                  colors={['#E6135A']}
                />
              }
            />
          </View>
        </SafeAreaView>
      )}
      {/* 알림 창 */}
      {showAlert && (
        <AlertComponent
          message={alertMessage}
          cancelUse={false}
          okPress={() => setShowAlert(false)}
        />
      )}
    </>
  )
}
