import React, {useCallback} from 'react'
import {StyleSheet, Text, View} from 'react-native'
import {SafeAreaView} from 'react-native-safe-area-context'
import {NavigationHeader, Icon} from '../components'
import {useNavigation} from '@react-navigation/native'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: 'white'
  },
  serviceInfoTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    marginTop: 10
  },
  infoText: {
    fontSize: 15,
    // marginTop: 30,
    borderBottomWidth: 1,
    borderColor: '#EEEEEE',
    paddingVertical: 15
  }
})

export default function ServiceInfo() {
  const navigation = useNavigation()

  const goBack = useCallback(() => {
    navigation.canGoBack() && navigation.goBack()
  }, [])

  return (
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
      />
      <View style={{flex: 1, paddingHorizontal: 20}}>
        <Text style={styles.serviceInfoTitle}>서비스 정보</Text>
        <Text style={{fontSize: 15, marginTop: 60}}>버전정보</Text>
        <Text
          style={{
            fontSize: 13,
            marginTop: 10,
            color: '#767676',
            borderBottomWidth: 1,
            borderColor: '#EEEEEE',
            paddingBottom: 15
          }}>
          1.2.2
        </Text>
        <Text
          style={[styles.infoText]}
          onPress={() => navigation.navigate('gaein')}>
          개인정보처리방침
        </Text>
        <Text
          style={[styles.infoText]}
          onPress={() => navigation.navigate('userClause')}>
          사용자 이용약관
        </Text>
        <Text
          style={[styles.infoText]}
          onPress={() => navigation.navigate('locateClause')}>
          위치정보 서비스 이용약관
        </Text>
      </View>
    </SafeAreaView>
  )
}
