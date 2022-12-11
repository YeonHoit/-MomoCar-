import React, {useEffect, useState, useCallback, useMemo, useRef} from 'react'
import {
  StyleSheet,
  View,
  TextInput,
  Dimensions,
  FlatList,
  Pressable,
  Keyboard,
  RefreshControl
} from 'react-native'
import {post} from '../server'
import type {CommunityPost} from '../type'
import {Icon, CommunityPostView, AlertComponent, Loading} from '../components'
import {useNavigation, useIsFocused} from '@react-navigation/native'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import FastImage from 'react-native-fast-image'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: '#F8F8FA'
  },
  text: {
    fontSize: 20
  },
  searchView: {
    width: '100%',
    height: 70,
    backgroundColor: 'white',
    borderBottomLeftRadius: 25,
    borderBottomRightRadius: 25,
    alignItems: 'center',
    justifyContent: 'center'
  },
  searchTextInputView: {
    width: Dimensions.get('window').width - 40,
    height: 45,
    borderWidth: 1,
    borderRadius: 30,
    borderColor: '#E6135A',
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 10
  },
  searchTextInput: {
    fontSize: 16,
    flex: 1,
    color: 'black',
    marginLeft: 5
  },
  floatingButton: {
    position: 'absolute',
    bottom: 10,
    right: 20,
    width: 60,
    height: 60,
    borderRadius: 30,
    backgroundColor: '#E6135A',
    alignItems: 'center',
    justifyContent: 'center'
  }
})

export default function FreeBoardCommunity() {
  // prettier-ignore
  const [communityPostList, setCommunityPostList] = useState<CommunityPost[]>([])
  const [isRefreshing, setIsRefreshing] = useState<boolean>(false)
  const [searchText, setSearchText] = useState<string>('')
  const [alertMessage, setAlertMessage] = useState<string>('')
  const [showAlert, setShowAlert] = useState<boolean>(false)
  //prettier-ignore
  const [communityPostListLoading, setCommunityPostListLoading] = useState<boolean>(true)

  const user = useSelector<AppState, Ur.State>((state) => state.user)
  const navigation = useNavigation()
  const isFocused = useIsFocused()

  const searchCommunityPostList = useMemo(() => {
    if (searchText.length == 0) {
      return [...communityPostList]
    } else {
      let arr: CommunityPost[] = []
      for (let i = 0; i < communityPostList.length; i++) {
        if (communityPostList[i].NickName.indexOf(searchText) != -1) {
          arr.push(communityPostList[i])
        } else if (communityPostList[i].Title.indexOf(searchText) != -1) {
          arr.push(communityPostList[i])
        } else if (communityPostList[i].Contents.indexOf(searchText) != -1) {
          arr.push(communityPostList[i])
        }
      }
      return arr
    }
  }, [searchText, communityPostList])

  const refresh = useCallback(() => {
    setIsRefreshing(true)
    let data = new FormData()
    data.append('BoardName', 'FreeBoard')
    data.append('UserID', user.ID)
    post('1.0.1/GetCommunityList.php', data)
      .then((CommunityInfo) => CommunityInfo.json())
      .then((CommunityInfoObject) => {
        const {GetCommunityList} = CommunityInfoObject
        setCommunityPostList(GetCommunityList)
      })
      .then(() => setIsRefreshing(false))
      .catch((e) => {
        setAlertMessage('글 목록을 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [user])

  useEffect(() => {
    let data = new FormData()
    data.append('BoardName', 'FreeBoard')
    data.append('UserID', user.ID)
    post('1.0.1/GetCommunityList.php', data)
      .then((CommunityInfo) => CommunityInfo.json())
      .then((CommunityInfoObject) => {
        const {GetCommunityList} = CommunityInfoObject
        setCommunityPostList(GetCommunityList)
      })
      .then(() => setCommunityPostListLoading(false))
      .catch((e) => {
        setAlertMessage('글 목록을 불러오지 못했습니다.')
        setShowAlert(true)
        setCommunityPostListLoading(false)
      })
  }, [user, isFocused])

  return (
    <>
      {communityPostListLoading && <Loading />}
      {!communityPostListLoading && (
        <View style={[styles.view]}>
          <View style={[styles.searchView]}>
            <View style={[styles.searchTextInputView]}>
              <TextInput
                style={[styles.searchTextInput]}
                placeholder="검색"
                placeholderTextColor="#767676"
                value={searchText}
                onChangeText={setSearchText}
              />
              <Icon
                source={require('../assets/images/magnify.png')}
                size={30}
                onPress={() => Keyboard.dismiss()}
              />
            </View>
          </View>
          <FlatList
            data={searchCommunityPostList}
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
      {user.ID !== 'guest' && (
        <Pressable
          style={[styles.floatingButton]}
          onPress={() =>
            navigation.navigate('WritePost', {boardName: 'FreeBoard'})
          }>
          <FastImage
            style={[{width: 40, height: 40}]}
            source={require('../assets/images/plus_white.png')}
          />
        </Pressable>
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
