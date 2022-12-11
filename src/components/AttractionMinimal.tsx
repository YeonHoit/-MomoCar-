import React, {useEffect, useState, useMemo} from 'react'
import type {FC} from 'react'
import {
  StyleSheet,
  ImageBackground,
  Text,
  View,
  Dimensions,
  TouchableOpacity
} from 'react-native'
import type {Attraction, AttractionReview} from '../type'
import {serverUrl} from '../server'
import FastImage from 'react-native-fast-image'
import {useNavigation} from '@react-navigation/native'
import {post} from '../server'

const styles = StyleSheet.create({
  backgroundView: {
    width: '100%',
    height: 350,
    backgroundColor: 'white'
  },
  backgroundImage: {
    width: Dimensions.get('window').width - 20,
    height: 330,
    borderRadius: 8,
    marginLeft: 10,
    marginTop: 10
  }
})

// 명소 정보 컴포넌트 매개변수 정의
export type AttractionMinimalProps = {
  attractionInfo: Attraction // 명소 정보 매개변수
  showDetail: boolean // 자세히 보기 텍스트 렌더링 여부 매개변수
}

// 명소 정보 컴포넌트 정의
export const AttractionMinimal: FC<AttractionMinimalProps> = ({
  attractionInfo,
  showDetail
}) => {
  const [reviewList, setReviewList] = useState<AttractionReview[]>([]) // 명소 리뷰 리스트 배열

  const navigation = useNavigation()

  // 명소에 대한 별점의 평균을 계산하여 반환하는 useMemo
  const starScore = useMemo(() => {
    let sum = 0
    if (reviewList.length == 0) {
      // 리뷰의 개수가 0개일경우, 0.0을 반환
      return sum.toFixed(1)
    }
    for (let i = 0; i < reviewList.length; i++) {
      // 명소의 별점 총합을 계산
      sum += reviewList[i].Star
    }
    // 명소의 평균 별점 반환
    return (sum / reviewList.length).toFixed(1)
  }, [reviewList])

  // 명소의 리뷰 리스트를 불러오는 useEffect
  useEffect(() => {
    let data = new FormData()
    data.append('Target', attractionInfo.Index_num)
    post('GetAttractionReviewList.php', data)
      .then((attractionReviewListInfo) => attractionReviewListInfo.json())
      .then((attractionReviewListInfoObject) => {
        const {GetAttractionReviewList} = attractionReviewListInfoObject
        setReviewList(GetAttractionReviewList)
      })
  }, [attractionInfo])

  // 명소 추천의 첫 화면 렌더링
  if (attractionInfo.Index_num == 0) {
    return (
      <ImageBackground
        style={[styles.backgroundView, {padding: 10}]}
        source={require('../assets/images/attraction_test3.jpeg')}
        imageStyle={[styles.backgroundImage]}>
        {/* 오른쪽 상단 더 알아보기 */}
        <View
          style={{flex: 1.7, flexDirection: 'row', justifyContent: 'flex-end'}}>
          <Text
            style={{
              color: 'white',
              fontSize: 12,
              textShadowColor: 'rgba(0, 0, 0, 0.9)',
              textShadowOffset: {width: -1, height: 1},
              textShadowRadius: 10,
              marginTop: 15,
              marginRight: 15
            }}
            onPress={() => navigation.navigate('AttractionScreen')}>
            더 알아보기 {'>'}
          </Text>
        </View>
        <Text
          style={{
            color: 'white',
            fontSize: 22,
            fontWeight: 'bold',
            textShadowColor: 'rgba(0, 0, 0, 0.9)',
            textShadowOffset: {width: -1, height: 1},
            textShadowRadius: 10,
            marginLeft: 15
          }}>
          드라이브 갈 곳이 없을 땐?
        </Text>
        <View style={{flex: 1}}>
          <Text
            style={{
              color: 'white',
              fontSize: 15,
              fontWeight: 'bold',
              marginTop: 10,
              textShadowColor: 'rgba(0, 0, 0, 0.9)',
              textShadowOffset: {width: -1, height: 1},
              textShadowRadius: 10,
              marginLeft: 15
            }}>
            모모카가 주변 명소를
          </Text>
          <Text
            style={{
              color: 'white',
              fontSize: 13,
              fontWeight: 'bold',
              marginTop: 5,
              textShadowColor: 'rgba(0, 0, 0, 0.9)',
              textShadowOffset: {width: -1, height: 1},
              textShadowRadius: 10,
              marginLeft: 15
            }}>
            추천해드려요
          </Text>
        </View>
      </ImageBackground>
    )
  }
  return (
    <TouchableOpacity
      onPress={() =>
        navigation.navigate('AttractionDetailScreen', {attractionInfo})
      }>
      {/* 명소 배경사진 */}
      <ImageBackground
        style={[styles.backgroundView, {padding: 10}]}
        source={{
          uri: serverUrl + 'img/attraction/' + attractionInfo.Photo[0].photo
        }}
        imageStyle={[styles.backgroundImage]}>
        {/* 오른쪽 상단 더 알아보기 */}
        <View style={{flex: 1, alignItems: 'flex-end'}}>
          <View style={{flexDirection: 'row', marginTop: 15, marginRight: 15}}>
            {showDetail && (
              <Text
                style={{
                  color: 'white',
                  fontSize: 12,
                  textShadowColor: 'rgba(0, 0, 0, 0.9)',
                  textShadowOffset: {width: -1, height: 1},
                  textShadowRadius: 10
                }}
                onPress={() => navigation.navigate('AttractionScreen')}>
                더 알아보기 {'>'}
              </Text>
            )}
          </View>
        </View>
        {/* 명소 이름과 평균 평점 */}
        <View style={{flex: 1, justifyContent: 'flex-end', paddingBottom: 55}}>
          <View
            style={{
              marginBottom: 5,
              marginLeft: 15,
              flexDirection: 'row',
              alignItems: 'center'
            }}>
            {/* 명소 이름 */}
            <Text
              style={{
                color: 'white',
                fontSize: 18,
                fontWeight: 'bold',
                textShadowColor: 'rgba(0, 0, 0, 0.9)',
                textShadowOffset: {width: -1, height: 1},
                textShadowRadius: 10
              }}>
              {attractionInfo.Name}
            </Text>
            {/* 명소의 평균 평점 */}
            <FastImage
              source={require('../assets/images/star_fill.png')}
              style={{width: 20, height: 20, marginRight: 5, marginLeft: 10}}
            />
            <Text style={{color: 'white'}}>{starScore}</Text>
          </View>
          {/* 명소의 설명 */}
          <Text
            style={{
              color: 'white',
              fontWeight: 'bold',
              marginLeft: 15,
              fontSize: 13,
              textShadowColor: 'rgba(0, 0, 0, 0.9)',
              textShadowOffset: {width: -1, height: 1},
              textShadowRadius: 10
            }}>
            {attractionInfo.Subtitle}
          </Text>
        </View>
      </ImageBackground>
    </TouchableOpacity>
  )
}
