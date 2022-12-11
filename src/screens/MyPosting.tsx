import React, {useState, useCallback, useEffect} from 'react'
import {StyleSheet, View, FlatList, RefreshControl} from 'react-native'
import type {CommunityPost} from '../type'
import {CommunityPostView, AlertComponent, Loading} from '../components'
import {post} from '../server'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: '#F8F8FA'
  }
})

export default function MyPosting() {
  //prettier-ignore
  const [communityPostList, setCommunityPostList] = useState<CommunityPost[]>([])
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false)
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(true)
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  const refresh = useCallback(() => {
    setIsRefreshing(true)
    let data = new FormData()
    data.append('NickName', user.Nickname)
    post('GetMyCommunity.php', data)
      .then((CommunityInfo) => CommunityInfo.json())
      .then((CommunityInfoObject) => {
        const {GetMyCommunity} = CommunityInfoObject
        setCommunityPostList(GetMyCommunity)
      })
      .then(() => setIsRefreshing(false))
      .catch((e) => {
        setAlertMessage('내가 쓴 글 목록을 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [user.Nickname])

  useEffect(() => {
    let data = new FormData()
    data.append('NickName', user.Nickname)
    post('GetMyCommunity.php', data)
      .then((CommunityInfo) => CommunityInfo.json())
      .then((CommunityInfoObject) => {
        const {GetMyCommunity} = CommunityInfoObject
        setCommunityPostList(GetMyCommunity)
      })
      .then(() => setLoading(false))
      .catch((e) => {
        setAlertMessage('내가 쓴 글 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setLoading(false)
      })
  }, [user.Nickname])

  return (
    <>
      {loading && <Loading />}
      {!loading && (
        <View style={[styles.view]}>
          <FlatList
            data={communityPostList}
            renderItem={({item}) => (
              <CommunityPostView communityPostInfo={item} />
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
      )}
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
