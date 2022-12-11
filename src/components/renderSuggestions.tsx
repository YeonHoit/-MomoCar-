import React from 'react'
import type {FC} from 'react'
import {Pressable, Text, View, StyleSheet} from 'react-native'
import {
  MentionSuggestionsProps,
  Suggestion
} from 'react-native-controlled-mentions'
import color from 'color'

const styles = StyleSheet.create({
  view: {
    backgroundColor: color('#E6135A').lighten(0.4).toString(),
    marginBottom: 5
  }
})

export const renderSuggestions: (
  suggestions: Suggestion[]
) => FC<MentionSuggestionsProps> =
  (suggestions) =>
  ({keyword, onSuggestionPress}) => {
    if (keyword == null) {
      return null
    }

    return (
      <View style={[styles.view]}>
        {suggestions
          .filter((one) =>
            one.name.toLocaleLowerCase().includes(keyword.toLocaleLowerCase())
          )
          .map((one) => (
            <Pressable
              key={one.id}
              onPress={() => onSuggestionPress(one)}
              style={{padding: 12}}>
              <Text style={[{color: 'white', fontWeight: 'bold'}]}>
                {one.name}
              </Text>
            </Pressable>
          ))}
      </View>
    )
  }
