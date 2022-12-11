import React, {useEffect, useState, useMemo} from 'react'
import type {FC, ComponentProps} from 'react'
import {StyleSheet, View, Text, Dimensions} from 'react-native'
import type {Comment} from '../type'
import moment from 'moment-with-locales-es6'
import {Icon, AlertComponent, TouchableImage} from '../components'
import {post, serverUrl} from '../server'
import {momentCal} from '../utils'

moment.locale('ko')

const styles = StyleSheet.create({
  view: {
    width: Dimensions.get('window').width,
    backgroundColor: 'white',
    paddingLeft: 20,
    paddingVertical: 10,
    flexDirection: 'row'
  },
  commentsText: {
    fontSize: 15,
    paddingVertical: 10,
    color: '#767676'
  },
  NickNameText: {
    fontSize: 15,
    paddingTop: 5,
    flex: 1,
    fontWeight: 'bold'
  },
  dateText: {
    fontSize: 13,
    color: '#767676'
  },
  profileImage: {
    width: 50,
    height: 50,
    borderRadius: 25
  }
})

type ViewProps = ComponentProps<typeof View>

// 댓글 컴포넌트 매개변수 정의
export type CommentViewProps = ViewProps & {
  commentInfo: Comment // 댓글 정보 매개변수
  iconPress: () => void // 신고, 차단등을 지원하는 아이콘 클릭 이벤트 함수
  tagPress: () => void // 댓글 태그 클릭 이벤트 함수
  profilePress: () => void // 댓글 작성자의 프로필 사진 클릭 이벤트 함수
}

// 댓글 컴포넌트 정의
export const CommentView: FC<CommentViewProps> = ({
  commentInfo,
  iconPress,
  tagPress,
  profilePress,
  ...viewProps
}) => {
  const [userPhoto, setUserPhoto] = useState<string>('') // 댓글 작성자의 프로필 사진 문자열
  const [alertMessage, setAlertMessage] = useState<string>('') // 알림 메세지 문자열
  const [showAlert, setShowAlert] = useState<boolean>(false) // 알림 표시 여부

  // 댓글 작성자의 프로필 사진 경로를 반환하는 useMemo
  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  // 댓글 작성자의 프로필 사진 경로 문자열을 가지고 오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('NickName', commentInfo.NickName)
    post('GetCommunityUserPhoto.php', data)
      .then((CommunityUserPhotoInfo) => CommunityUserPhotoInfo.json())
      .then((CommunityUserPhotoInfoObject) => {
        const {GetCommunityUserPhoto} = CommunityUserPhotoInfoObject
        setUserPhoto(GetCommunityUserPhoto[0].Photo)
      })
      .catch((e) => {
        setAlertMessage('사용자의 정보를 가져오지 못했습니다.')
        setShowAlert(true)
      })
  }, [commentInfo])

  return (
    <>
      <View style={[styles.view]} {...viewProps}>
        <View>
          {/* 댓글 작성자의 프로필 사진 */}
          <TouchableImage
            source={profileImage}
            style={[styles.profileImage]}
            onPress={profilePress}
          />
        </View>
        <View style={[{flex: 1, marginLeft: 15}]}>
          {/* 댓글 작성자의 닉네임 */}
          <View style={[{flexDirection: 'row'}]}>
            <Text style={[styles.NickNameText]}>{commentInfo.NickName}</Text>
          </View>
          {/* 댓글 내용 */}
          <Text style={[styles.commentsText]}>{commentInfo.Comments}</Text>
          <View style={{flexDirection: 'row', alignItems: 'center'}}>
            {/* 댓글 작성 시간 */}
            <Text style={[styles.dateText]}>
              {momentCal(commentInfo.CommentsDateTime)}
            </Text>
            {/* 댓글 작성자 태그 */}
            <Text
              style={[styles.dateText, {marginLeft: 20}]}
              onPress={tagPress}>
              답글달기
            </Text>
          </View>
        </View>
        <View
          style={[{width: 50, alignItems: 'center', justifyContent: 'center'}]}>
          {/* 신고, 차단등을 지원하는 아이콘 */}
          <Icon
            source={require('../assets/images/three_dots_vertical.png')}
            size={30}
            onPress={iconPress}
          />
        </View>
      </View>
      {/* 알림 메세지 */}
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
