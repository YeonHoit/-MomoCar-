import React, {useEffect, useState, useMemo, useCallback} from 'react'
import type {FC, ComponentProps} from 'react'
import {StyleSheet, View, Text, Dimensions} from 'react-native'
import type {CommunityPost} from '../type'
import AutoHeightImage from 'react-native-auto-height-image'
import moment from 'moment-with-locales-es6'
import {useNavigation} from '@react-navigation/native'
import {post, serverUrl} from '../server'
import {AlertComponent} from './AlertComponent'
import {momentCal, priceComma} from '../utils'
import {TouchableView} from './TouchableView'
import {useSelector} from 'react-redux'
import type {AppState} from '../store'
import * as Ur from '../store/user'
import FastImage from 'react-native-fast-image'
import {Icon} from '../components'

moment.locale('ko')

const styles = StyleSheet.create({
  view: {
    width: '100%',
    backgroundColor: 'white',
    paddingVertical: 10,
    paddingHorizontal: 20,
    marginTop: 10
  },
  text: {
    fontSize: 16
  },
  topView: {
    width: '100%',
    height: 60,
    flexDirection: 'row'
  },
  profileImage: {
    width: 60,
    height: 60,
    borderRadius: 30
  },
  nicknameText: {
    fontSize: 15,
    padding: 5,
    color: 'black'
  },
  dateText: {
    fontSize: 12,
    paddingLeft: 5,
    color: '#767676'
  },
  viewsText: {
    fontSize: 10,
    paddingTop: 5,
    color: '#767676'
  },
  titleText: {
    width: '100%',
    marginTop: 15,
    borderBottomWidth: 1,
    borderColor: '#DBDBDB',
    paddingVertical: 5
  },
  contentsText: {
    marginTop: 10,
    fontSize: 14,
    lineHeight: 20,
    color: '#191919'
  },
  pressableButton: {
    width: '100%',
    flexDirection: 'row',
    justifyContent: 'flex-end',
    paddingVertical: 10
  },
  pressableButtonText: {
    fontSize: 12,
    color: '#767676'
  },
  heart_comment_text: {
    fontSize: 13,
    paddingLeft: 5,
    color: '#767676'
  },
  priceView: {
    marginTop: 10,
    paddingVertical: 10,
    flexDirection: 'row',
    alignItems: 'center'
  },
  wonText: {
    fontSize: 16,
    fontWeight: 'bold',
    color: 'black'
  },
  priceTextInput: {
    fontSize: 16,
    marginLeft: 10,
    color: '#767676'
  }
})

type ViewProps = ComponentProps<typeof View>

// 커뮤니티 글 컴포넌트 매개변수 정의
export type CommunityPostViewProps = ViewProps & {
  communityPostInfo: CommunityPost // 커뮤니티 글 정보 매개변수
}

// 커뮤니티 글 컴포넌트 정의
export const CommunityPostView: FC<CommunityPostViewProps> = ({
  communityPostInfo,
  ...viewProps
}) => {
  const [likeCount, setLikeCount] = useState<number>(0) // 좋아요 개수
  const [commentCount, setCommentCount] = useState<number>(0) // 댓글 개수
  const [alertMessage, setAlertMessage] = useState<string>('') // 알림 메세지 문자열
  const [showAlert, setShowAlert] = useState<boolean>(false) // 알림 메세지 표시 여부
  const [userPhoto, setUserPhoto] = useState<string>('') // 글 작성자의 프로필 이미지 문자열
  const [like, setLike] = useState<boolean>(false) // 좋아요 버튼 활성화 여부

  const navigation = useNavigation()
  const user = useSelector<AppState, Ur.State>((state) => state.user)

  // 커뮤니티 글 작성자의 프로필 이미지 경로를 반환하는 useMemo
  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  // 커뮤니티 글의 첫번째 첨부 사진 컴포넌트를 반환하는 useMemo
  const firstPhoto = useMemo(() => {
    if (communityPostInfo.Photo.length != 0) {
      let arr = communityPostInfo.Photo.split('|')
      return (
        <AutoHeightImage
          width={Dimensions.get('window').width - 40}
          source={{uri: serverUrl + 'img/' + arr[0]}}
          style={[{marginTop: 10}]}
        />
      )
    }
    return null
  }, [communityPostInfo])

  // 좋아요 버튼을 눌렀을 때 실행되는 함수
  const likeButtonPress = useCallback(() => {
    if (!like) {
      // 좋아요가 활성화 되어 있지 않을 때
      setLikeCount(+likeCount + 1)
      let data = new FormData()
      data.append('ID', user.ID)
      data.append('CommunityNumber', communityPostInfo.Index_num)
      post('InsertLike.php', data).then(() => {
        if (user.Nickname !== communityPostInfo.NickName) {
          let data = new FormData()
          data.append('Title', '새로운 좋아요')
          data.append('Contents', '회원님의 게시물에 새로운 좋아요가 있습니다.')
          data.append('AlertType', 'Like')
          data.append('FromUser', user.Nickname)
          data.append('ToUser', communityPostInfo.NickName)
          data.append('Target_num', communityPostInfo.Index_num)
          post('InsertAlert.php', data)
        }
      })
    } else {
      // 좋아요가 활성화 되어 있을 때
      setLikeCount(likeCount - 1)
      let data = new FormData()
      data.append('ID', user.ID)
      data.append('CommunityNumber', communityPostInfo.Index_num)
      post('DeleteLike.php', data)
    }
    setLike(!like)
  }, [user, like, communityPostInfo, likeCount])

  // 좋아요 개수를 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityPostInfo.Index_num)
    post('GetLikeCount.php', data)
      .then((LikeCountInfo) => LikeCountInfo.json())
      .then((LikeCountInfoObject) => {
        const {GetLikeCount} = LikeCountInfoObject
        setLikeCount(GetLikeCount[0].Count)
      })
      .catch((e) => {
        setAlertMessage('좋아요 숫자를 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [communityPostInfo])

  // 댓글 개수를 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('CommunityNumber', communityPostInfo.Index_num)
    post('GetCommentCount.php', data)
      .then((CommentCountInfo) => CommentCountInfo.json())
      .then((CommentCountInfoObject) => {
        const {GetCommentCount} = CommentCountInfoObject
        setCommentCount(GetCommentCount[0].Count)
      })
      .catch((e) => {
        setAlertMessage('댓글 숫자를 불러오지 못했습니다.')
        setShowAlert(true)
      })
  }, [communityPostInfo])

  // 커뮤니티 글 작성자의 프로필 이미지 경로 문자열을 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('NickName', communityPostInfo.NickName)
    post('GetCommunityUserPhoto.php', data)
      .then((CommunityUserPhotoInfo) => CommunityUserPhotoInfo.json())
      .then((CommunityUserPhotoInfoObject) => {
        const {GetCommunityUserPhoto} = CommunityUserPhotoInfoObject
        setUserPhoto(GetCommunityUserPhoto[0].Photo)
      })
  }, [communityPostInfo])

  // 사용자가 커뮤니티 글에 좋아요를 눌렀는지 검색한 결과를 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('ID', user.ID)
    data.append('CommunityNumber', communityPostInfo.Index_num)
    post('GetLikeSearch.php', data)
      .then((LikeInfo) => LikeInfo.json())
      .then((LikeInfoObject) => {
        const {GetLikeSearch} = LikeInfoObject
        if (GetLikeSearch[0]) {
          setLike(true)
        } else {
          setLike(false)
        }
      })
  }, [communityPostInfo, user])

  return (
    <>
      <TouchableView
        viewStyle={[styles.view]}
        {...viewProps}
        onPress={() =>
          navigation.navigate('PostDetailView', {
            communityNumber:
              communityPostInfo.Index_num.toString() +
              '|' +
              new Date().toString()
          })
        }>
        <View style={[styles.topView]}>
          {/* 글 작성자의 프로필 이미지 */}
          <FastImage style={[styles.profileImage]} source={profileImage} />
          <View style={[{flex: 1, marginLeft: 5}]}>
            {/* 글 작성자의 닉네임 */}
            <Text style={[styles.nicknameText]}>
              {communityPostInfo.NickName}
            </Text>
            {/* 글 작성 시간 */}
            <Text style={[styles.dateText]}>
              {momentCal(communityPostInfo.WriteDate)}
            </Text>
          </View>
          {/* 글 조회 수 */}
          <Text style={[styles.viewsText]}>조회 {communityPostInfo.Views}</Text>
        </View>
        {/* 글 제목 */}
        <View style={[styles.titleText]}>
          <Text style={[{fontSize: 15, fontWeight: '700'}]}>
            {communityPostInfo.Title}
          </Text>
        </View>
        {/* 중고장터 품목의 가격 */}
        {communityPostInfo.BoardName === 'UsedMarket' && (
          <View style={[styles.priceView]}>
            <Text style={[styles.wonText]}>￦</Text>
            <Text style={[styles.priceTextInput]}>
              {priceComma(communityPostInfo.Price)}
            </Text>
          </View>
        )}
        {/* 글 내용 (최대 6줄) */}
        <Text
          style={[styles.contentsText]}
          numberOfLines={6}
          ellipsizeMode="tail">
          {communityPostInfo.Contents}
        </Text>
        {/* 글 첫번째 첨부 사진 */}
        {firstPhoto}
        <View style={[styles.pressableButton]} />
        <View style={[{flexDirection: 'row'}]}>
          {user.ID === 'guest' && (
            <Icon
              size={30}
              source={require('../assets/images/heart_empty.png')}
              onPress={() => {
                setAlertMessage('로그인이 필요한 서비스입니다.')
                setShowAlert(true)
              }}
            />
          )}
          {/* 좋아요 비 활성화 아이콘 */}
          {user.ID !== 'guest' && !like && (
            <Icon
              size={30}
              source={require('../assets/images/heart_empty.png')}
              onPress={likeButtonPress}
            />
          )}
          {/* 좋아요 활성화 아이콘 */}
          {user.ID !== 'guest' && like && (
            <Icon
              size={30}
              source={require('../assets/images/heart_fill.png')}
              onPress={likeButtonPress}
            />
          )}
          {/* 좋아요 개수 */}
          <View style={[{height: 30, justifyContent: 'center'}]}>
            <Text style={[styles.heart_comment_text]}>{likeCount}</Text>
          </View>
          {/* 댓글 아이콘 */}
          <FastImage
            style={[{width: 30, height: 30, marginLeft: 50}]}
            source={require('../assets/images/comment.png')}
          />
          {/* 댓글 개수 */}
          <View style={[{height: 30, justifyContent: 'center'}]}>
            <Text style={[styles.heart_comment_text]}>{commentCount}</Text>
          </View>
          <View style={{flex: 1}} />
        </View>
      </TouchableView>
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
