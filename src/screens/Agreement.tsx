import React, {useState, useEffect, useCallback} from 'react'
import {StyleSheet, Text, Dimensions, View} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import Icon from 'react-native-vector-icons/MaterialCommunityIcons'
import {TouchableView} from '../components'
import {useNavigation} from '@react-navigation/native'
import {useToggle} from '../hooks'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white',
    paddingHorizontal: 20
  },
  titleText: {
    fontSize: 22,
    fontWeight: 'bold',
    marginTop: 50
  },
  touchableView: {
    width: Dimensions.get('window').width - 40,
    backgroundColor: '#EEEEEE',
    marginTop: 20,
    borderRadius: 10,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 15,
    paddingVertical: 20
  },
  listView: {
    width: Dimensions.get('window').width - 40,
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 20,
    paddingHorizontal: 10
  },
  detailTouchView: {
    width: 40,
    height: 20,
    borderRadius: 5,
    backgroundColor: '#DBDBDB',
    alignItems: 'center',
    justifyContent: 'center'
  },
  nextButton: {
    width: Dimensions.get('window').width - 40,
    height: 50,
    borderRadius: 10,
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 20
  }
})

// 약관 동의를 받는 화면
export default function Agreement() {
  const [total, setTotal] = useState<boolean>(false) // 모든 약관 동의 여부
  const [gaein, toggleGaein] = useToggle() // 개인정보처리방침 동의 여부
  const [user, toggleUser] = useToggle() // 사용자 이용약관 동의 여부
  const [locate, toggleLocate] = useToggle() // 위치 정보 서비스 이용약관 동의 여부
  const [fourteen, toggleFourteen] = useToggle() // 만 14세 이상 동의 여부
  const navigation = useNavigation()

  // 모든 약관을 동의하는 버튼 클릭 이벤트 함수
  const totalPress = useCallback(() => {
    if (!gaein) {
      toggleGaein()
    }
    if (!user) {
      toggleUser()
    }
    if (!locate) {
      toggleLocate()
    }
    if (!fourteen) {
      toggleFourteen()
    }
  }, [gaein, user, locate, fourteen])

  // 모든 약관이 동의되었을때를 구분하는 useEffect
  useEffect(() => {
    if (gaein && user && locate && fourteen) {
      setTotal(true)
    } else {
      setTotal(false)
    }
  }, [gaein, user, locate, fourteen])

  return (
    <SafeAreaView style={[styles.view]}>
      <Text style={[styles.titleText]}>약관에 동의해주세요</Text>
      {/* 전체 약관 동의 버튼 */}
      <TouchableView viewStyle={[styles.touchableView]} onPress={totalPress}>
        {!total && (
          <Icon size={30} name="check-circle-outline" color="#E6135A" />
        )}
        {total && <Icon size={30} name="check-circle" color="#E6135A" />}
        <Text style={{fontSize: 16, marginLeft: 10, fontWeight: 'bold'}}>
          전체 약관에 동의합니다.
        </Text>
      </TouchableView>
      {/* 개인정보처리방침 동의 버튼 */}
      <TouchableView viewStyle={[styles.listView]} onPress={toggleGaein}>
        {!gaein && <Icon size={20} name="check" color="#DBDBDB" />}
        {gaein && <Icon size={20} name="check" color="#E6135A" />}
        <Text style={{marginLeft: 20}}>개인정보처리방침에 동의합니다.</Text>
        <View style={{flex: 1}} />
        <TouchableView
          viewStyle={[styles.detailTouchView]}
          onPress={() => navigation.navigate('gaein')}>
          <Text style={{fontSize: 12}}>보기</Text>
        </TouchableView>
      </TouchableView>
      {/* 사용자 이용약관 동의 버튼 */}
      <TouchableView viewStyle={[styles.listView]} onPress={toggleUser}>
        {!user && <Icon size={20} name="check" color="#DBDBDB" />}
        {user && <Icon size={20} name="check" color="#E6135A" />}
        <Text style={{marginLeft: 20}}>사용자 이용약관에 동의합니다.</Text>
        <View style={{flex: 1}} />
        <TouchableView
          viewStyle={[styles.detailTouchView]}
          onPress={() => navigation.navigate('userClause')}>
          <Text style={{fontSize: 12}}>보기</Text>
        </TouchableView>
      </TouchableView>
      {/* 위치정보 서비스 이용약관 동의 버튼 */}
      <TouchableView viewStyle={[styles.listView]} onPress={toggleLocate}>
        {!locate && <Icon size={20} name="check" color="#DBDBDB" />}
        {locate && <Icon size={20} name="check" color="#E6135A" />}
        <Text style={{marginLeft: 20}}>
          위치정보 서비스 이용약관에 동의합니다.
        </Text>
        <View style={{flex: 1}} />
        <TouchableView
          viewStyle={[styles.detailTouchView]}
          onPress={() => navigation.navigate('locateClause')}>
          <Text style={{fontSize: 12}}>보기</Text>
        </TouchableView>
      </TouchableView>
      {/* 만 14세 이상 동의 버튼 */}
      <TouchableView viewStyle={[styles.listView]} onPress={toggleFourteen}>
        {!fourteen && <Icon size={20} name="check" color="#DBDBDB" />}
        {fourteen && <Icon size={20} name="check" color="#E6135A" />}
        <Text style={{marginLeft: 20}}>만 14세 이상입니다.</Text>
      </TouchableView>
      <View style={{flex: 1}} />
      {/* 다음 화면으로 넘어가는 버튼 */}
      {!total && (
        <View style={[styles.nextButton, {backgroundColor: '#D9D9DF'}]}>
          <Text style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
            동의하고 시작하기
          </Text>
        </View>
      )}
      {total && (
        <TouchableView
          viewStyle={[styles.nextButton, {backgroundColor: '#E6135A'}]}
          onPress={() => navigation.navigate('EditNickName')}>
          <Text style={[{color: 'white', fontWeight: '700', fontSize: 14}]}>
            동의하고 시작하기
          </Text>
        </TouchableView>
      )}
    </SafeAreaView>
  )
}
