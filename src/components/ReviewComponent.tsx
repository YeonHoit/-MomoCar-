import React, {useEffect, useState, useMemo} from 'react'
import type {FC} from 'react'
import {StyleSheet, View, Text, Pressable} from 'react-native'
import type {Review} from '../type'
import {post} from '../server'
import {momentCal} from '../utils'
import {serverUrl} from '../server'
import FastImage from 'react-native-fast-image'
import {useNavigation} from '@react-navigation/native'

const styles = StyleSheet.create({
  view: {
    width: '100%',
    marginTop: 15,
    borderBottomWidth: 1,
    borderBottomColor: '#DBDBDB'
  },
  infoView: {
    width: '100%',
    flexDirection: 'row'
  },
  profileImage: {
    width: 60,
    height: 60,
    borderRadius: 30
  },
  star: {
    width: 15,
    height: 15,
    marginRight: 5
  },
  dateText: {
    fontSize: 12,
    color: '#999999'
  },
  contentsText: {
    marginVertical: 15,
    paddingHorizontal: 5
  },
  exampleImage: {
    width: 75,
    height: 75
  }
})

// 리뷰 컴포넌트 매개변수 정의
export type ReviewComponentProps = {
  reviewInfo: Review // 리뷰 정보
}

// 리뷰 컴포넌트 정의
export const ReviewComponent: FC<ReviewComponentProps> = ({reviewInfo}) => {
  const [writer, setWriter] = useState<string>('') // 작성자 문자열
  const [userPhoto, setUserPhoto] = useState<string>('') // 작성자 이미지 문자열

  const navigation = useNavigation()

  // 리뷰 사진 목록 컴포넌트를 반환하는 useMemo
  const photoList = useMemo(() => {
    let temp = reviewInfo.Photo.split('|')
    temp.pop()
    return temp.map((item, index) => (
      <Pressable
        style={[styles.exampleImage, {marginRight: 10}]}
        key={index.toString()}
        onPress={() => navigation.navigate('PhotoDetail', {photo: item})}>
        <FastImage
          style={[styles.exampleImage]}
          source={{uri: serverUrl + 'img/' + item}}
        />
      </Pressable>
    ))
  }, [reviewInfo])

  // 별점 컴포넌트를 반환하는 useMemo
  const star = useMemo(() => {
    // 점수별 배열 만들기
    let check: boolean[] = [false, false, false, false, false]
    if (reviewInfo.Star == 1) {
      check = [true, false, false, false, false]
    } else if (reviewInfo.Star == 2) {
      check = [true, true, false, false, false]
    } else if (reviewInfo.Star == 3) {
      check = [true, true, true, false, false]
    } else if (reviewInfo.Star == 4) {
      check = [true, true, true, true, false]
    } else if (reviewInfo.Star == 5) {
      check = [true, true, true, true, true]
    }
    return check.map((bool, index) => {
      if (bool) {
        return (
          <FastImage
            style={[styles.star]}
            source={require('../assets/images/star_fill.png')}
            key={index.toString()}
          />
        )
      }
      return (
        <FastImage
          style={[styles.star]}
          source={require('../assets/images/star_empty.png')}
          key={index.toString()}
        />
      )
    })
  }, [reviewInfo])

  // 작성자 프로필 이미지 경로를 반환하는 useMemo
  const profileImage = useMemo(() => {
    if (userPhoto.length > 0) {
      return {uri: serverUrl + 'img/' + userPhoto}
    }
    return require('../assets/images/emptyProfileImage.png')
  }, [userPhoto])

  // 작성자 닉네임을 가져오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('ID', reviewInfo.WriterID)
    post('GetUserNickName.php', data)
      .then((UserNickNameInfo) => UserNickNameInfo.json())
      .then((UserNickNameInfoObject) => {
        const {GetUserNickName} = UserNickNameInfoObject
        setWriter(GetUserNickName[0].Nickname)
      })
  }, [reviewInfo])

  // 작성자 프로필 이미지 경로 문자열을 가지고 오는 useEffect
  useEffect(() => {
    if (writer.length > 0) {
      let data = new FormData()
      data.append('NickName', writer)
      post('GetCommunityUserPhoto.php', data)
        .then((UserPhotoInfo) => UserPhotoInfo.json())
        .then((UserPhotoInfoObject) => {
          const {GetCommunityUserPhoto} = UserPhotoInfoObject
          setUserPhoto(GetCommunityUserPhoto[0].Photo)
        })
    }
  }, [writer])

  return (
    <View style={[styles.view]}>
      <View style={[styles.infoView]}>
        {/* 작성자 프로필 이미지 */}
        <FastImage style={[styles.profileImage]} source={profileImage} />
        <View style={{flex: 1, marginLeft: 10, marginTop: 5}}>
          {/* 작성자 닉네임 */}
          <Text>{writer}</Text>
          {/* 별점 */}
          <View style={{flexDirection: 'row', marginTop: 10}}>{star}</View>
        </View>
        {/* 작성 시간 */}
        <Text style={[styles.dateText]}>{momentCal(reviewInfo.WriteDate)}</Text>
      </View>
      {/* 리뷰 사진 */}
      <View style={{flexDirection: 'row', marginTop: 15}}>{photoList}</View>
      {/* 리뷰 내용 */}
      <Text style={[styles.contentsText]}>{reviewInfo.Contents}</Text>
    </View>
  )
}
